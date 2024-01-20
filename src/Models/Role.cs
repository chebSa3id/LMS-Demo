namespace LMS_Demo.src.Models;
public class Role
{
	[Key]
	public int Id { get; set; }
	[Required]
	public string Name { get; set; }
	public IEnumerable<User> Users { get; set; }
}