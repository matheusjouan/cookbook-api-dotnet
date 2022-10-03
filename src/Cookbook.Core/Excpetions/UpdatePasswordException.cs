using Microsoft.AspNetCore.Http;

namespace Cookbook.Core.Excpetions;

public class UpdatePasswordException : ExceptionsBase
{
	public UpdatePasswordException()
	{
		Error = new Error
		{
			Title = "Updat ePassword Exception",
			Status = StatusCodes.Status400BadRequest,
			Detail = "The current password is wrong."
		};
	}
}
