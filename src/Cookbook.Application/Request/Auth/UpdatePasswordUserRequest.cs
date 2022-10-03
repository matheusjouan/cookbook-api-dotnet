namespace Cookbook.Application.Request.Auth;

public class UpdatePasswordUserRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
