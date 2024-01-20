namespace LMS_Demo.src.Repos;
public interface IEnrollmentRepo
{
	Task<Enrollment> GetEnrollmentById(Enrollment enrollment);
	Task<IEnumerable<Enrollment>> GetEnrollments();
	Task<Enrollment> CreateEnrollment(Enrollment enrollment);
	Task<Enrollment> UpdateEnrollment(Enrollment enrollment);
	Task<Enrollment> DeleteEnrollment(Enrollment enrollment);
}