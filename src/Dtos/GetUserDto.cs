namespace LMS_Demo.src.Dtos
{
	public class GetUserDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int RoleId { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public RoleDto Role { get; set; }
		public ICollection<EnrollmentDto> Enrollments { get; set; }
	}
}