using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;

namespace HaloBiz.MyServices
{
    public interface IOfficeService
    {
        Task<ApiCommonResponse> AddOffice(OfficeReceivingDTO officeReceivingDTO);
        Task<ApiCommonResponse> GetAllOffices();
        Task<ApiCommonResponse> GetOfficeById(long id);
        Task<ApiCommonResponse> GetOfficeByName(string name);
        Task<ApiCommonResponse> UpdateOffice(long id, OfficeReceivingDTO branchReceivingDTO);
        Task<ApiCommonResponse> DeleteOffice(long id);

    }
}