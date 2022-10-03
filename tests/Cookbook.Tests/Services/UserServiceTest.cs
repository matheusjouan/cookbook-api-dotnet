using AutoMapper;
using Cookbook.Application.Response.User;
using Cookbook.Application.Services;
using Cookbook.Application.Services.Interfaces;
using Cookbook.Core.Entities;
using Cookbook.Core.Excpetions;
using Cookbook.Core.Interfaces;
using Cookbook.Tests.Configuration;
using Cookbook.Tests.Fixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace Cookbook.Tests.Services;

public class UserServiceTest
{
    private readonly IUserService _sut;
    private readonly UserFixture _userFixture;
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ICryptographyService> _cryptMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAcessor;

    public UserServiceTest()
    {
        _mapper = AutoMapperConfig.GetConfiguration();
        _uowMock = new Mock<IUnitOfWork>();
        _cryptMock = new Mock<ICryptographyService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _httpContextAcessor = new Mock<IHttpContextAccessor>();
        _userFixture = new UserFixture();

        _sut = new UserServices(_uowMock.Object, _mapper, _cryptMock.Object,
            _tokenServiceMock.Object, _httpContextAcessor.Object);
    }

    [Fact]
    public async Task CreateUser_WhenUserIsValid_ReturnToken()
    {
        // Arrange
        var userToCreate = _userFixture.EntryCreateUserRequest();
        var tokenGenerate = _userFixture.GenerateTokenValid();
        var cryptoPassword = _userFixture.GenerateString();

        _uowMock.Setup(_ => _.UserRepository.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        _cryptMock.Setup(_ => _.CryptographyPassword(It.IsAny<string>()))
            .Returns(cryptoPassword);

        _tokenServiceMock.Setup(_ => _.GenerateToken(It.IsAny<string>()))
            .Returns(tokenGenerate);

        // Act
        var result = await _sut.CreateUserAsync(userToCreate);

        // Assert
        _uowMock.Verify(_ => _.SaveChangeAsync(), Times.Once);
        result.Should().BeEquivalentTo(tokenGenerate);
    }

    [Fact]
    public async Task CreateUser_WhenEmailAlreadyExists_ReturnsEmailAlreadyExistsException()
    {
        // Arrange 
        var userToCreate = _userFixture.EntryCreateUserRequest();
        var userExists = _mapper.Map<User>(userToCreate);

        _uowMock.Setup(_ => _.UserRepository.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(userExists);

        // Act
        Func<Task<SignInResponse>> act = async () => await _sut.CreateUserAsync(userToCreate);

        // Assert
        await act.Should()
            .ThrowAsync<EmailAlreadyExistsException>();
    }

    [Fact]
    public async Task SignIn_WhenCredentialsValid_ReturnsToken()
    {
        // Arrange
        var userRequest = _userFixture.EntryCreateUserRequest();
        var user = _mapper.Map<User>(userRequest);
        var passwordHash = _userFixture.GenerateString();
        var tokenGenerate = _userFixture.GenerateTokenValid();
        var credentials = _userFixture.GenerateCredentialsUser();

        _cryptMock.Setup(_ => _.CryptographyPassword(It.IsAny<string>()))
            .Returns(passwordHash);

        _uowMock.Setup(_ => _.UserRepository.AuthenticationUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(user);

        _tokenServiceMock.Setup(_ => _.GenerateToken(It.IsAny<string>()))
            .Returns(tokenGenerate);

        // Act
        var result = await _sut.AuthenticateUserAsync(credentials);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(tokenGenerate);
    }

    [Fact]
    public async Task SignIn_WhenCredentialsInvalid_ReturnsException()
    {
        // Arrange
        var passwordHash = _userFixture.GenerateString();
        var credentials = _userFixture.GenerateCredentialsUser();

        _cryptMock.Setup(_ => _.CryptographyPassword(It.IsAny<string>()))
         .Returns(passwordHash);

        _uowMock.Setup(_ => _.UserRepository.AuthenticationUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(() => null);

        // Act
        Func<Task<SignInResponse>> act = async () => await _sut.AuthenticateUserAsync(credentials);

        // Assert
        await act.Should()
            .ThrowAsync<CredentialsInvalidException>();
    }

    [Fact]
    public async Task UpdatePassword_WhenCurrentPasswordValid_ReturnUpdatedPassword()
    {
        // Arrange
        var request = _userFixture.GenerateUpdatePasswordRequest();
        var userDto = _userFixture.EntryCreateUserRequest();
        var user = _mapper.Map<User>(userDto);
        var newPassword = "123456";

        var identity = new GenericIdentity("claim", "claim");
        var context = new DefaultHttpContext() { User = new ClaimsPrincipal(identity) };

        _httpContextAcessor.Setup(_ => _.HttpContext)
            .Returns(context);

        _uowMock.Setup(_ => _.UserRepository.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _cryptMock.SetupSequence(_ => _.CryptographyPassword(It.IsAny<string>()))
            .Returns(user.Password)
            .Returns(newPassword);  

        // Act
        await _sut.UpdatePasswordAsync(request);

        // Assert
        _uowMock.Verify(_ => _.SaveChangeAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePassword_WhenCurrentPasswordInvalid_ReturnsException()
    {
        // Arrange
        var request = _userFixture.GenerateUpdatePasswordRequest();
        var userDto = _userFixture.EntryCreateUserRequest();
        var user = _mapper.Map<User>(userDto);
        var anyPassword = "7896548";
        var newPassword = "123456";


        var identity = new GenericIdentity("claim", "claim");
        var context = new DefaultHttpContext() { User = new ClaimsPrincipal(identity) };

        _httpContextAcessor.Setup(_ => _.HttpContext)
            .Returns(context);

        _uowMock.Setup(_ => _.UserRepository.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _cryptMock.SetupSequence(_ => _.CryptographyPassword(It.IsAny<string>()))
            .Returns(anyPassword)
            .Returns(newPassword);

        // Act
        Func<Task> act = async() => await _sut.UpdatePasswordAsync(request);

        // Assert
        _uowMock.Verify(_ => _.SaveChangeAsync(), Times.Never);
        await act.Should().ThrowAsync<UpdatePasswordException>();
    }
}
