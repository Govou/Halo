using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;

namespace HaloBiz.MyServices
{
    public interface IServiceCategoryService
    {
        Task<ApiCommonResponse> AddServiceCategory(ServiceCategoryReceivingDTO serviceCategoryReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceCategory();
        Task<ApiCommonResponse> GetServiceCategoryById(long id);
        Task<ApiCommonResponse> GetServiceCategoryByName(string name);
        Task<ApiCommonResponse> UpdateServiceCategory(long id, ServiceCategoryReceivingDTO serviceCategoryReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceCategory(long id);
        
    }
}