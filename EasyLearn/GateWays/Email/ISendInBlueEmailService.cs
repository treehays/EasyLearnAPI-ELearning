using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.EmailSenderDTOs;
using EasyLearn.Models.DTOs.UserDTOs;

namespace EasyLearn.GateWays.Email;

public interface ISendInBlueEmailService
{
    Task<BaseResponse<UserDTO>> SendEmailWithoutAttachment(EmailSenderNoAttachmentDTO model);
    Task<BaseResponse<UserDTO>> EmailVerificationTemplate(EmailSenderDetails model, string baseUrl);
    Task<BaseResponse<UserDTO>> CourseVerificationTemplate(EmailSenderDetails model, string baseUrl);
    Task<BaseResponse<UserDTO>> EnrollmentEmailTemplate(EmailSenderDetails model, string baseUrl);
    Task<BaseResponse<UserDTO>> CourseCompletionEmailTemplate(EmailSenderDetails model, string baseUrl);
    Task<BaseResponse<UserDTO>> WithdrawalConfirmationEmailTemplate(EmailSenderDetails model, string baseUrl);
    //Task<BaseResponse> SendEmailAttachment(EmailSenderAttachmentDTO model);

}
