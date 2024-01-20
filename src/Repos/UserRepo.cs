namespace LMS_Demo.src.Repos;
public class UserRepo : IUserRepo
{
	private readonly DataContext _context;
	public UserRepo(DataContext context)
	{
		_context = context;
	}

	public async Task<User> GetUserById(int id)
	{
		return await _context.Users.Include(x => x.Role).Include(x => x.Enrollments).ThenInclude(y => y.Course).FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<IEnumerable<User>> GetUsersByIds(List<int> ids)
	{
		return await _context.Users.Where(x => ids.Contains(x.Id)).ToListAsync();
	}

	public async Task<User> GetUserByEmail(string email)
	{
		return await _context.Users.Include(x => x.Role).Include(x => x.Enrollments).FirstOrDefaultAsync(x => x.Email == email);
	}

	public async Task<IEnumerable<User>> GetUsers()
	{
		return await _context.Users.ToListAsync();
	}

	public async Task<User> CreateUser(User user)
	{
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		return user;
	}

	public async Task<User> UpdateUser(User user, int id)
	{
		var userToUpdate = await _context.Users.FindAsync(id);

		if (userToUpdate == null)
			return null;

		userToUpdate.Email = user.Email;
		userToUpdate.Password = user.Password;
		userToUpdate.FirstName = user.FirstName;
		userToUpdate.LastName = user.LastName;
		userToUpdate.RoleId = user.RoleId;

		await _context.SaveChangesAsync();

		return userToUpdate;
	}
	public async Task<User> DeleteUser(int id)
	{
		var user = await _context.Users.FindAsync(id);

		if (user == null)
			return null;

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return user;
	}
}
