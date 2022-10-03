using Microsoft.AspNetCore.Http;

namespace Cookbook.Core.Excpetions;

public class EmailAlreadyExistsException : ExceptionsBase
{
	public EmailAlreadyExistsException()
	{
		Error = new Error
		{
			Title = "Email already exists",
			Status = StatusCodes.Status400BadRequest,
			Detail = "Email already in use"
		};
	}
}
