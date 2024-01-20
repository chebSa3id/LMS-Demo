namespace LMS_Demo.src.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IUserRepo _userRepo;
	private readonly IMapper _mapper;
	private readonly Serilog.ILogger _logger;
	private const string TokenKey = "ThisismycustomSecretkeyforauthenticationandmustbestoredmoresecurly";
	private static readonly TimeSpan TokenExpiration = TimeSpan.FromHours(1);

	public UserController(IUserRepo userRepo, IMapper mapper, Serilog.ILogger logger)
	{
		_userRepo = userRepo;
		_mapper = mapper;
		_logger = logger;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginDto loginDto)
	{
		var user = await _userRepo.GetUserByEmail(loginDto.Email);

		if (user == null)
			return NotFound();

		// Create a PasswordHasher
		var hasher = new PasswordHasher<User>();

		// Verify the provided password with the hashed password
		var verificationResult = hasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

		if (verificationResult == PasswordVerificationResult.Failed)
		{
			_logger.Information("User with id: {id} has failed to login", user.Id);
			return Unauthorized("Invalid Email or Password");
		}


		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(TokenKey);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim("Name", user.FirstName+' '+user.LastName),
				new Claim("Email", user.Email),
				new Claim("UserRole", user.Role.Name)
			}),
			Expires = DateTime.UtcNow.Add(TokenExpiration),
			Issuer = "http://localhost:5001",
			Audience = "http://localhost:5001",
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenString = tokenHandler.WriteToken(token);
		_logger.Information("User with id: {id} has logged in", user.Id);
		return Ok(new { Message = "Login Succeeded", Token = tokenString });
	}

	[HttpPost("register")]
	public async Task<IActionResult> CreateUser(CreateUserDto userDto)
	{
		var user = _mapper.Map<User>(userDto);
		var hasher = new PasswordHasher<User>();
		user.Password = hasher.HashPassword(user, userDto.Password);
		await _userRepo.CreateUser(user);
		var userToReturn = _mapper.Map<GetUserDto>(user);
		_logger.Information("User with id: {id} has been created", userToReturn.Id);
		return CreatedAtAction(nameof(GetUserById), new { id = userToReturn.Id }, userToReturn);
	}



	[Authorize]
	[AuthorizationFilter(new string[] { "Admin", "Student" })]
	[HttpGet]
	public async Task<IActionResult> GetAllUsers()
	{
		var users = await _userRepo.GetUsers();
		var usersDto = _mapper.Map<IEnumerable<GetUserDto>>(users);
		return Ok(usersDto);
	}

	[Authorize]
	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserById(int id)
	{
		var user = await _userRepo.GetUserById(id);
		var userDto = _mapper.Map<GetUserDto>(user);
		return Ok(userDto);
	}

	[Authorize]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUser(int id, CreateUserDto userDto)
	{
		var user = _mapper.Map<User>(userDto);
		await _userRepo.UpdateUser(user, id);
		_logger.Information("User with id: {id} has been updated", id);
		return NoContent();
	}

	[Authorize]
	[AuthorizationFilter(new string[] { "Admin" })]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		await _userRepo.DeleteUser(id);
		_logger.Information("User with id: {id} has been deleted", id);
		return NoContent();
	}
}