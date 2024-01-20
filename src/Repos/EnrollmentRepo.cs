namespace LMS_Demo.src.Repos;
public class EnrollmentRepo : IEnrollmentRepo
{
	private readonly DataContext _context;
	public EnrollmentRepo(DataContext context)
	{
		_context = context;
	}

	public async Task<Enrollment> GetEnrollmentById(Enrollment enrollment)
	{
		return await _context.Enrollments.Include(y => y.Course).Where(x => x.CourseId == enrollment.CourseId && x.UserId == enrollment.UserId).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<Enrollment>> GetEnrollments()
	{
		return await _context.Enrollments.ToListAsync();
	}

	public async Task<Enrollment> CreateEnrollment(Enrollment enrollment)
	{
		enrollment.Grade = 0;
		await _context.Enrollments.AddAsync(enrollment);
		await _context.SaveChangesAsync();

		return enrollment;
	}

	public async Task<Enrollment> UpdateEnrollment(Enrollment enrollment)
	{
		var enrollmentToUpdate = await _context.Enrollments.Where(x => x.CourseId == enrollment.CourseId && x.UserId == enrollment.UserId).FirstOrDefaultAsync();

		if (enrollmentToUpdate == null)
			return null;

		enrollmentToUpdate.IsEnrolled = enrollment.IsEnrolled;
		enrollmentToUpdate.IsPaid = enrollment.IsPaid;
		enrollmentToUpdate.ReceiptNumber = enrollment.ReceiptNumber;
		enrollmentToUpdate.Grade = enrollment.Grade;
		await _context.SaveChangesAsync();

		return enrollmentToUpdate;
	}

	public async Task<Enrollment> DeleteEnrollment(Enrollment enrollment)
	{
		var enrollmentToUpdate = await _context.Enrollments.Where(x => x.CourseId == enrollment.CourseId && x.UserId == enrollment.UserId).FirstOrDefaultAsync();

		if (enrollment == null)
			return null;

		_context.Enrollments.Remove(enrollment);
		await _context.SaveChangesAsync();

		return enrollment;
	}
}