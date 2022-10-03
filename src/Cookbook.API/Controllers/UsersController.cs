using Cookbook.Application.Request.Auth;
using Cookbook.Application.Response.User;
using Cookbook.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("new-password")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status204NoContent)]

    public async Task<IActionResult> UpdatePassword(UpdatePasswordUserRequest request)
    {
        await _userService.UpdatePasswordAsync(request);
        return NoContent();
    }


    [HttpGet("profile")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAuthenticatedInfo()
    {
        var user = await _userService.GetUserAuthenticatedInfoAsync();
        return Ok(user);
    }

}
