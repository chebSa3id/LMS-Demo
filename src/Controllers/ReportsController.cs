namespace LMS_Demo.src.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IUserRepo _userRepo;
	private readonly ICourseRepo _courseRepo;
	private readonly IEnrollmentRepo _enrollmentRepo;
	public ReportsController(IMapper mapper, IUserRepo userRepo, ICourseRepo courseRepo, IEnrollmentRepo enrollmentRepo)
	{
		_mapper = mapper;
		_userRepo = userRepo;
		_courseRepo = courseRepo;
		_enrollmentRepo = enrollmentRepo;
	}

	[Authorize]
	[HttpGet("GetCourseUsers/{courseId}")]
	public async Task<IActionResult> GetCourseUsers(int courseId)
	{
		var course = await _courseRepo.GetCourseById(courseId);

		if (course == null)
			return NotFound();

		var enrolledUsersIds = course.Enrollments.Select(e => e.UserId);
		var users = await _userRepo.GetUsersByIds(enrolledUsersIds.ToList());
		var usersDto = _mapper.Map<IEnumerable<GetUserDto>>(users);
		return Ok(usersDto);
	}

	[Authorize]
	[HttpGet("GetUserGrades/{userId}")]
	public async Task<IActionResult> GetUserGrades(int userId)
	{
		var user = await _userRepo.GetUserById(userId);

		if (user == null)
			return NotFound();

		var userGrades = user.Enrollments.Select(e => new { e.Course.Title, e.Grade });
		return Ok(userGrades);
	}
}
