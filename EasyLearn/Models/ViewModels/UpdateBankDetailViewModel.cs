using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.PaymentDetailDTOs;

namespace EasyLearn.Models.ViewModels;

public class UpdateBankDetailViewModel
{
    public BaseResponse<ICollection<PaymentDetailDTO>> PaymentDetail { get; set; }

}
