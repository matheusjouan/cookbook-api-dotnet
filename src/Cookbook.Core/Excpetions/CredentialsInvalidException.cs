using Microsoft.AspNetCore.Http;

namespace Cookbook.Core.Excpetions;

public class CredentialsInvalidException : ExceptionsBase
{
	public CredentialsInvalidException()
	{
		Error = new Error
		{
			Title = "Credentials Invalid",
			Status = StatusCodes.Status401Unauthorized,
			Detail = "Invalid email and/or password"
        };
	}
}
