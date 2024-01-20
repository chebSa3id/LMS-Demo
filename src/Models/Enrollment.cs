namespace LMS_Demo.src.Models;
[PrimaryKey("UserId", "CourseId")]
public class Enrollment
{
	[ForeignKey("User"), Required]
	public int UserId { get; set; }
	[ForeignKey("Course"), Required]
	public int CourseId { get; set; }

	[Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
	public int Grade { get; set; } = 0;
	public string? ReceiptNumber { get; set; }
	public bool IsPaid { get; set; } = false;
	public bool IsEnrolled { get; set; } = false;
	public User User { get; set; }
	public Course Course { get; set; }
}