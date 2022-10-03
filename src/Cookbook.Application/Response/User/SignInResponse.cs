namespace Cookbook.Application.Response.User;

public class SignInResponse
{
    public string Token { get; set; }
    public DateTime ExpiresIn { get; set; }
}
