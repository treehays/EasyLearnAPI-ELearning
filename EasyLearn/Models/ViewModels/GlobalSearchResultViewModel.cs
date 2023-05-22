using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.CategoryDTOs;
using EasyLearn.Models.DTOs.CourseDTOs;
using EasyLearn.Models.DTOs.InstructorDTOs;

namespace EasyLearn.Models.ViewModels;

public class GlobalSearchResultViewModel
{
    public BaseResponse<ICollection<CategoryDTO>> CategoriesResponseModel { get; set; }
    public BaseResponse<ICollection<InstructorDto>> InstructorsResponseModel { get; set; }
    public BaseResponse<ICollection<CourseDTO>> CoursesResponseModel { get; set; }
}
