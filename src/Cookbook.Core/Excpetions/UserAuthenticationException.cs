using Microsoft.AspNetCore.Http;

namespace Cookbook.Core.Excpetions;
public class UserAuthenticationException : ExceptionsBase
{
	public UserAuthenticationException()
	{
		Error = new Error
		{
			Title = "AuthenticationException",
			Status = StatusCodes.Status401Unauthorized,
			Detail = "User not Authenticated"
		};
	}
}
