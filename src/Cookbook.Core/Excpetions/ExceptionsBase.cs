namespace Cookbook.Core.Excpetions;

public class ExceptionsBase : Exception
{
	public Error Error { get; protected set; }
	public ExceptionsBase() { }
	public ExceptionsBase(string message) : base(message) { }
	public ExceptionsBase(string message, Exception innerException) : base(message, innerException) { }
}
