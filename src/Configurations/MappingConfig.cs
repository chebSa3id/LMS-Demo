namespace LMS_Demo.src.Configurations;

public class MappingConfig : Profile
{
	public MappingConfig()
	{
		CreateMap<GetUserDto, User>().ReverseMap();
		CreateMap<CreateUserDto, User>().ReverseMap();
		CreateMap<LoginDto, User>().ReverseMap();
		CreateMap<EnrollmentDto, Enrollment>().ReverseMap();
		CreateMap<CreateCourseDto, Course>().ReverseMap();
		CreateMap<GetCourseDto, Course>().ReverseMap();
		CreateMap<RoleDto, Role>().ReverseMap();
	}
}