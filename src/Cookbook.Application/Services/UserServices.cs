using AutoMapper;
using Cookbook.Application.Request.Auth;
using Cookbook.Application.Response.User;
using Cookbook.Application.Services.Interfaces;
using Cookbook.Core.Entities;
using Cookbook.Core.Excpetions;
using Cookbook.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cookbook.Application.Services;

public class UserServices : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ICryptographyService _cryptographyService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserServices(IUnitOfWork uow, IMapper mapper,
        ICryptographyService cryptographyService, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor)
    {
        _uow = uow;
        _mapper = mapper;
        _cryptographyService = cryptographyService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SignInResponse> AuthenticateUserAsync([FromBody] SignInRequest request)
    {
        // Criptografar a senha p/ comparar com a do BD
        var passwordHash = _cryptographyService.CryptographyPassword(request.Password);

        var user = await _uow.UserRepository.AuthenticationUserAsync(request.Email,
            passwordHash);

        if (user == null)
            throw new CredentialsInvalidException();

        var token = _tokenService.GenerateToken(user.Email);

        return token;
    }

    public async Task<SignInResponse> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var userExists = await GetUserByEmailAsync(request.Email);

        if (userExists != null)
            throw new EmailAlreadyExistsException();


        var newUser = _mapper.Map<User>(request);
        newUser.Password = _cryptographyService.CryptographyPassword(request.Password);

        _uow.UserRepository.Add(newUser);
        await _uow.SaveChangeAsync();

        var token = _tokenService.GenerateToken(request.Email);

        return token;
    }

    public async Task UpdatePasswordAsync(UpdatePasswordUserRequest request)
    {
        // Obtem o e-mail do token JWT
        var email = _httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.Email);

        // Obtendo os dados do usuário através do e-mail
        var user = await GetUserByEmailAsync(email);

        // Verificando se a senha passada é igual a atual
        var currentPassword = _cryptographyService.CryptographyPassword(request.CurrentPassword);
        if (!currentPassword.Equals(user.Password))
            throw new UpdatePasswordException();

        var newPasswordHash = _cryptographyService.CryptographyPassword(request.NewPassword);
        user.Password = newPasswordHash;

        _uow.UserRepository.Update(user);
        await _uow.SaveChangeAsync();
    }

    public async Task<UserAuthenticatedResponse> GetUserAuthenticatedInfoAsync()
    {
        // Obtendo email do usuário através da claims
        var email = _httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.Email);

        var user = await GetUserByEmailAsync(email);

        return _mapper.Map<UserAuthenticatedResponse>(user);
    }

    private async Task<User> GetUserByEmailAsync(string email) =>
         await _uow.UserRepository.GetUserByEmailAsync(email);
}