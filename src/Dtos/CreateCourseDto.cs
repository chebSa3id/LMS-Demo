namespace LMS_Demo.src.Dtos
{
	public class CreateCourseDto
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public int Credits { get; set; }
	}
}