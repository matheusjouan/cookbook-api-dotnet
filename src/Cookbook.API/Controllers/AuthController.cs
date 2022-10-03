using Cookbook.Application.Request.Auth;
using Cookbook.Application.Response.User;
using Cookbook.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cookbook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> SignUp(CreateUserRequest request)
    {
        var token = await _userService.CreateUserAsync(request);
        return Created(string.Empty, token);
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        var token = await _userService.AuthenticateUserAsync(request);
        return Ok(token);
    }
}
