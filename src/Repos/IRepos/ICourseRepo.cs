namespace LMS_Demo.src.Repos;
public interface ICourseRepo
{
	Task<Course> GetCourseById(int id);
	Task<IEnumerable<Course>> GetCourses();
	Task<Course> CreateCourse(Course course);
	Task<Course> UpdateCourse(Course course, int id);
	Task<Course> DeleteCourse(int id);
}