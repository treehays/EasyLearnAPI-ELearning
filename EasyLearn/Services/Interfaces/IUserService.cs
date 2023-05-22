using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.UserDTOs;

namespace EasyLearn.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponse<UserDTO>> UserRegistration(CreateUserRequestModel model, string baseUrl, string userId);
    Task<BaseResponse<UserDTO>> EmailVerification(string emailToken, string userId);
    Task<BaseResponse<UserDTO>> UpgradeUserRole(UserUpgradeRequestModel model, string userId);
    Task<BaseResponse<LoginModel>> Login(LoginRequestModel model);
    Task<BaseResponse<UserDTO>> PasswordReset(string email, string baseUrl);
    Task<BaseResponse<UserDTO>> ConfirmPasswordReset(string emailToken, string userId);
    Task<BaseResponse<UserDTO>> DeleteUser(string id, string userId);
    Task<BaseResponse<UserDTO>> GetUserById(string id);
    Task<BaseResponse<ICollection<UserDTO>>> GetAllUser();
    Task<BaseResponse<UserDTO>> UpdateUserProfile(UpdateUserProfileRequestModel model, string userId);
}
