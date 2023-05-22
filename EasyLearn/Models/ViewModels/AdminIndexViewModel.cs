using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.CourseDTOs;
using EasyLearn.Models.DTOs.EnrolmentDTOs;

namespace EasyLearn.Models.ViewModels;

public class AdminIndexViewModel
{
    public BaseResponse<ICollection<EnrolmentDTO>> Enrolments { get; set; }
    public BaseResponse<ICollection<CourseDTO>> Courses { get; set; }
}
