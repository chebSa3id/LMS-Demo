namespace LMS_Demo.src.Models;
public class Course
{
	[Key]
	public int Id { get; set; }

	[StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
	[Required]
	public string Title { get; set; }

	[StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
	public string Description { get; set; }

	[Required]
	[Range(1, 10000, ErrorMessage = "Credits must be between 1 and 10000.")]
	public int Credits { get; set; }

	public IEnumerable<Enrollment> Enrollments { get; set; }
}