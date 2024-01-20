namespace LMS_Demo.src.Repos
{
	public class CourseRepo : ICourseRepo
	{
		private readonly DataContext _context;
		public CourseRepo(DataContext context)
		{
			_context = context;
		}

		public async Task<Course> GetCourseById(int id)
		{
			return await _context.Courses.Include(x => x.Enrollments).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Course>> GetCourses()
		{
			return await _context.Courses.ToListAsync();
		}

		public async Task<Course> CreateCourse(Course course)
		{
			await _context.Courses.AddAsync(course);
			await _context.SaveChangesAsync();

			return course;
		}

		public async Task<Course> UpdateCourse(Course course, int id)
		{
			var courseToUpdate = await _context.Courses.FindAsync(id);

			if (courseToUpdate == null)
				return null;

			courseToUpdate.Credits = course.Credits;
			courseToUpdate.Description = course.Description;
			courseToUpdate.Title = course.Title;
			await _context.SaveChangesAsync();

			return courseToUpdate;
		}

		public async Task<Course> DeleteCourse(int id)
		{
			var course = await _context.Courses.FindAsync(id);

			if (course == null)
				return null;

			_context.Courses.Remove(course);
			await _context.SaveChangesAsync();

			return course;
		}
	}
}