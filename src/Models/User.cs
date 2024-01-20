namespace LMS_Demo.src.Models;
public class User
{
	[Key]
	public int Id { get; set; }

	[Required]
	[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
	public string FirstName { get; set; }

	[Required]
	[StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
	public string LastName { get; set; }

	[Required]
	[EmailAddress(ErrorMessage = "Invalid Email Address")]
	public string Email { get; set; }

	[Required]
	[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
	public string Password { get; set; }

	[Required]
	public int RoleId { get; set; }
	public Role Role { get; set; }
	public IEnumerable<Enrollment> Enrollments { get; set; }
}
