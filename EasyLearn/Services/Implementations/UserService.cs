using BCrypt.Net;
using EasyLearn.GateWays.Email;
using EasyLearn.GateWays.FileManager;
using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.EmailSenderDTOs;
using EasyLearn.Models.DTOs.UserDTOs;
using EasyLearn.Models.Entities;
using EasyLearn.Models.Enums;
using EasyLearn.Repositories.Interfaces;
using EasyLearn.Services.Interfaces;
using Mapster;

namespace EasyLearn.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISendInBlueEmailService _emailService;
    private readonly IFileManagerService _fileManagerService;
    public UserService(IUserRepository userRepository, ISendInBlueEmailService emailService, IFileManagerService fileManagerService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _fileManagerService = fileManagerService;
    }
    public async Task<BaseResponse> DeleteUser(string id, string userId)
    {

        var user = await _userRepository.GetAsync(x => x.Id == id && !x.IsDeleted && x.IsActive);
        if (user == null)
        {
            return new BaseResponse
            {
                Message = "User not Found...",
                Success = false,
            };
        }

        var date = DateTime.Now;

        user.IsDeleted = true;
        user.DeletedOn = date;
        user.DeletedBy = userId;
        //user.Address.IsDeleted = true;
        //user.Address.DeletedOn = date;
        //user.Address.DeletedBy = userId;
        //user.Admin.IsDeleted = true;
        //user.Admin.DeletedOn = date;
        //user.Admin.DeletedBy = userId;

        await _userRepository.SaveChangesAsync();
        return new BaseResponse
        {
            Message = "Admin successfully deleted...",
            Success = true,
        };

    }

    public async Task<UsersResponseModel> GetAllUser()
    {
        var users1 = await _userRepository.GetAllAsync();
        var users = await _userRepository.GetListAsync(x => x.RoleId == "Admin");

        if (users.Count() == 0)
        {
            return new UsersResponseModel
            {
                Message = "User not found..",
                Success = false,
            };
        }

        var userModel = new UsersResponseModel
        {
            Success = true,
            Message = "Details successfully retrieved...",
            Data = users.Adapt<IList<UserDTO>>(),
        };
        return userModel;
    }

    public async Task<UserResponseModel> GetUserById(string id)
    {
        var user = await _userRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
        {
            return new UserResponseModel
            {
                Message = "User not found...",
                Success = false,
            };
        }
        var userDto = new UserResponseModel
        {
            Message = "Details successfully retrieved...",
            Success = true,
            Data = user.Adapt<UserDTO>(),
        };
        return userDto;
    }

    public async Task<UserResponseModel> UpdateUserProfile(UpdateUserProfileRequestModel model, string userId)
    {
        var user = await _userRepository.GetAsync(x => x.Id == model.Id && x.IsActive && !x.IsDeleted);
        if (user == null)
        {
            return new UserResponseModel
            {
                Message = "User does not exist",
                Success = false,
                //Data = user.Adapt<UserDTO>(),
            };
        }


        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.ProfilePicture = model.ProfilePicture ?? user.ProfilePicture;
        user.Biography = model.Biography ?? user.Biography;
        user.Skill = model.Skill ?? user.Skill;
        user.Interest = model.Interest ?? user.Interest;
        user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
        user.StudentshipStatus = model.StudentshipStatus != 0 ? StudentshipStatus.Student : StudentshipStatus.Graduate;
        user.ModifiedOn = DateTime.Now;
        user.ModifiedBy = userId;
        await _userRepository.SaveChangesAsync();
        return new UserResponseModel
        {
            Message = "Account created successfully",
            Success = true,
            Data = user.Adapt<UserDTO>(),
        };
    }

    public async Task<BaseResponse> UserRegistration(CreateUserRequestModel model, string baseUrl, string userId)
    {
        var emailExist = await _userRepository.ExistByEmailAsync(model.Email);
        if (emailExist)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Email already exit..",
            };
        }
        var filePath = await _fileManagerService.GetFileName(model.FormFile, "uploads", "images", "profilePictures");
        var userName = model.Email.Remove(model.Email.IndexOf('@'));
        var password = BCrypt.Net.BCrypt.HashPassword(model.Password, SaltRevision.Revision2Y);
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Password = password,
            Gender = model.Gender,
            StudentshipStatus = model.StudentshipStatus,

            UserName = $"{userName}{new Random().Next(100, 999)}",
            CreatedOn = DateTime.Now,
            IsActive = true,
            EmailToken = Guid.NewGuid().ToString().Replace('-', '0'),
            RoleId = "Student",
            ProfilePicture = filePath,
            CreatedBy = userId,
        };

        var userStudent = new Student
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            CreatedBy = user.CreatedBy,
            CreatedOn = user.CreatedOn,
        };
        var senderDetail = new EmailSenderDetails
        {
            EmailToken = $"{baseUrl}/User/ConfirmEmail?emailToken={user.EmailToken}",
            ReceiverEmail = user.Email,
            ReceiverName = model.FirstName,
        };
        var emailSender = await _emailService.EmailVerificationTemplate(senderDetail, baseUrl);
        user.Student = userStudent;
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return new BaseResponse
        {
            Success = true,
            Message = "Account successfuly created..",
        };
    }
}
