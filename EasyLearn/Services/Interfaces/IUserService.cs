using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.UserDTOs;
using EasyLearn.Models.Entities;

namespace EasyLearn.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponse> UserRegistration(CreateUserRequestModel model, string baseUrl, string userId);
    Task<BaseResponse> DeleteUser(string id, string userId);
    Task<UserResponseModel> GetUserById(string id);
    Task<UsersResponseModel> GetAllUser();
    Task<UserResponseModel> UpdateUserProfile(UpdateUserProfileRequestModel model, string userId);
}
