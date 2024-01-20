
namespace LMS_Demo.src.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IEnrollmentRepo _repo;

	public record CheckoutSessionRequest(int userId, int courseId);

	public EnrollmentController(IMapper mapper, IEnrollmentRepo repo)
	{
		_mapper = mapper;
		_repo = repo;
	}

	[Authorize]
	[HttpPost("checkout")]
	public async Task<ActionResult> CreateCheckoutSession(CheckoutSessionRequest payload)
	{
		var tempEnrollment = new Enrollment
		{
			UserId = payload.userId,
			CourseId = payload.courseId,
		};

		var enrollment = await _repo.GetEnrollmentById(tempEnrollment);
		if (enrollment == null)

			return NotFound();

		StripeConfiguration.ApiKey = "sk_test_4eC39HqLyjWDarjtT1zdp7dc";

		var priceOptions = new PriceCreateOptions
		{
			Currency = "usd",
			UnitAmount = enrollment.Course.Credits,
			ProductData = new PriceProductDataOptions { Name = enrollment.Course.Title },
		};
		var priceService = new PriceService();
		var priceData = priceService.Create(priceOptions);



		var baseUrl = $"{Request.Scheme}://{Request.Host}";

		var sessionOptions = new SessionCreateOptions
		{
			LineItems = new List<SessionLineItemOptions>
						{
								new SessionLineItemOptions
								{
										Price = priceData.Id,
										Quantity = 1,
								},
						},
			Mode = "payment",
			SuccessUrl = baseUrl + $"/success",
			CancelUrl = baseUrl,
		};

		var sessionService = new SessionService();
		var session = await sessionService.CreateAsync(sessionOptions);

		enrollment.ReceiptNumber = session.Id;
		await _repo.UpdateEnrollment(enrollment);

		return Ok(new { SessionId = session.Id, URL = session.Url });
	}

	[Authorize]
	[HttpPost("validate_payment")]
	public async Task<IActionResult> ValidatePayment(EnrollmentDto EnrollmentDto)
	{
		StripeConfiguration.ApiKey = "sk_test_4eC39HqLyjWDarjtT1zdp7dc";

		var service = new SessionService();
		var session = await service.GetAsync(EnrollmentDto.ReceiptNumber);
		if (session.PaymentStatus == "paid")
		{
			var enrollment = _mapper.Map<Enrollment>(EnrollmentDto);
			enrollment.IsPaid = true;
			await _repo.UpdateEnrollment(enrollment);
		}
		return Ok(session.PaymentStatus);
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpPost("confirm_enrollment")]
	public async Task<IActionResult> ConfirmEnrollment(EnrollmentDto enrollmentDto)
	{

		var enrollment = await _repo.GetEnrollmentById(_mapper.Map<Enrollment>(enrollmentDto));

		if (!enrollment.IsPaid)
			return BadRequest("Enrollment is not paid");

		enrollment.IsEnrolled = true;
		await _repo.UpdateEnrollment(enrollment);
		return Ok("Enrollment confirmed");
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpGet("enrollments")]
	public async Task<IActionResult> GetEnrollments()
	{
		var enrollments = await _repo.GetEnrollments();
		var enrollmentsDto = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
		return Ok(enrollmentsDto);
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpPost("get_enrollment")]
	public async Task<IActionResult> GetEnrollment(EnrollmentDto createEnrollmentDto)
	{

		var enrollment = await _repo.GetEnrollmentById(_mapper.Map<Enrollment>(createEnrollmentDto));

		if (enrollment == null)
			return NotFound();

		var enrollmentDto = _mapper.Map<EnrollmentDto>(enrollment);
		return Ok(enrollmentDto);
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreateEnrollment(EnrollmentDto createEnrollmentDto)
	{
		var enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);
		await _repo.CreateEnrollment(enrollment);
		var enrollmentDto = _mapper.Map<EnrollmentDto>(enrollment);
		return Ok(enrollmentDto);
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpPut]
	public async Task<IActionResult> UpdateEnrollment(EnrollmentDto createEnrollmentDto)
	{
		var enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);
		await _repo.UpdateEnrollment(enrollment);
		return NoContent();
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpDelete]
	public async Task<IActionResult> DeleteEnrollment(EnrollmentDto enrollmentDto)
	{
		var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
		await _repo.DeleteEnrollment(enrollment);
		return NoContent();
	}
}