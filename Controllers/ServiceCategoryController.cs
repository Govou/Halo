using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        
        private readonly IServiceCategoryService _serviceCategoryService ;

        public ServiceCategoryController(IServiceCategoryService serviceCategoryService)
        {
            this._serviceCategoryService = serviceCategoryService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServiceCategories()
        {
            return await _serviceCategoryService.GetAllServiceCategory();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _serviceCategoryService.GetServiceCategoryByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _serviceCategoryService.GetServiceCategoryById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNew(ServiceCategoryReceivingDTO serviceCategoryReceivingDTO)
        {
            return await _serviceCategoryService.AddServiceCategory(serviceCategoryReceivingDTO);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceCategoryReceivingDTO serviceCategoryReceivingDTO)
        {
            return await _serviceCategoryService.UpdateServiceCategory(id, serviceCategoryReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _serviceCategoryService.DeleteServiceCategory(id);
        }
    }
}