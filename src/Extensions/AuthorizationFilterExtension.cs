using Microsoft.AspNetCore.Mvc.Filters;

namespace LMS_Demo.src.Extensions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
	private readonly string[] _role;

	public AuthorizationFilterAttribute(string[] role)
	{
		_role = role;
	}
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var userRole = context.HttpContext.User.Claims.ToList().Where(y => y.Type == "UserRole").Select(x => x.Value).FirstOrDefault();
		if (_role.Length == 0 || !_role.Contains(userRole))
		{
			context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
			return;
		}
	}
}