namespace LMS_Demo.src.Repos;
public interface IUserRepo
{
	Task<User> GetUserById(int id);
	Task<IEnumerable<User>> GetUsersByIds(List<int> ids);
	Task<User> GetUserByEmail(string email);
	Task<IEnumerable<User>> GetUsers();
	Task<User> CreateUser(User user);
	Task<User> UpdateUser(User user, int id);
	Task<User> DeleteUser(int id);
}