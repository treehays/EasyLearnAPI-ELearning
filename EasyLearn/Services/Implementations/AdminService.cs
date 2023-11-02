using EasyLearn.Repositories.Interfaces;
using EasyLearn.Services.Interfaces;

namespace EasyLearn.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public AdminService(IUserRepository userRepository, IUserService userService) => (_userRepository, _userService) = (userRepository, userService);
    //{
    //    _userRepository = userRepository;
    //    userRepositoryuserRepository = userService;
    //}

    //public async Task<BaseResponse> AdminRegistration(CreateUserRequestModel model, string baseUrl, string userId)
    //{
    //    var admin = await _userService.UserRegistration(model, baseUrl, userId);
    //    if (admin == null)
    //    {
    //        return new BaseResponse
    //        {
    //            Success = false,
    //            Message = "Email already exist.",
    //        };
    //    }

    //    var userAdmin = new Admin
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        UserId = admin.Id,
    //        CreatedBy = admin.CreatedBy,
    //        CreatedOn = admin.CreatedOn,
    //    };
    //    admin.Admin = userAdmin;
    //    admin.RoleId = "Admin";
    //    await _userRepository.AddAsync(admin);
    //    await _userRepository.SaveChangesAsync();


    //    return new BaseResponse
    //    {
    //        Status = true,
    //        Message = "Account successfully created.",
    //    };
    //}
}
