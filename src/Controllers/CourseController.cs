namespace LMS_Demo.src.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly ICourseRepo _repo;
	public CourseController(IMapper mapper, ICourseRepo repo)
	{
		_mapper = mapper;
		_repo = repo;
	}

	[HttpGet]
	public async Task<IActionResult> GetCourses()
	{
		var courses = await _repo.GetCourses();
		var coursesDto = _mapper.Map<IEnumerable<GetCourseDto>>(courses);
		return Ok(coursesDto);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCourse(int id)
	{
		var course = await _repo.GetCourseById(id);

		if (course == null)
			return NotFound();

		var courseDto = _mapper.Map<GetCourseDto>(course);
		return Ok(courseDto);
	}



	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpPost]
	public async Task<IActionResult> CreateCourse(CreateCourseDto createCourseDto)
	{
		var course = _mapper.Map<Course>(createCourseDto);
		await _repo.CreateCourse(course);
		var courseDto = _mapper.Map<GetCourseDto>(course);
		return CreatedAtAction(nameof(GetCourse), new { id = courseDto.Id }, courseDto);
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateCourse(int id, CreateCourseDto createCourseDto)
	{
		var course = _mapper.Map<Course>(createCourseDto);
		await _repo.UpdateCourse(course, id);
		return NoContent();
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCourse(int id)
	{
		await _repo.DeleteCourse(id);
		return NoContent();
	}
}