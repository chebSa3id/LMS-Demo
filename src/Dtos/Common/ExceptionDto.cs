
namespace LMS_Demo.src.Dtos.Common;

public class ExceptionDto
{
	public ExceptionDto(int statusCode, string message = null, string details = null)
	{
		StatusCode = statusCode;
		Message = message;
		Details = details;
	}

	public int StatusCode { get; set; }
	public string Message { get; set; }
	public string Details { get; set; }
}