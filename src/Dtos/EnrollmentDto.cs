namespace LMS_Demo.src.Dtos
{
	public class EnrollmentDto
	{
		public int UserId { get; set; }
		public int CourseId { get; set; }
		public int Grade { get; set; }
		public string ReceiptNumber { get; set; }
		public bool IsPaid { get; }
		public bool IsEnrolled { get; }
		public GetCourseDto Course { get; set; }
	}
}