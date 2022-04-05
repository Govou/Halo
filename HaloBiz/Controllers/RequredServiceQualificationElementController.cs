using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class RequredServiceQualificationElementController : ControllerBase
    {
        private readonly IRequredServiceQualificationElementService _RequredServiceQualificationElementService;

        public RequredServiceQualificationElementController(IRequredServiceQualificationElementService RequredServiceQualificationElementService)
        {
            this._RequredServiceQualificationElementService = RequredServiceQualificationElementService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetRequredServiceQualificationElement()
        {
            return await _RequredServiceQualificationElementService.GetAllRequredServiceQualificationElements();
        }

        [HttpGet("GetByServiceCategory")]
        public async Task<ApiCommonResponse> GetRequredServiceQualificationElementByServiceCategory(long serviceCategoryId)
        {
            return await _RequredServiceQualificationElementService.GetAllRequredServiceQualificationElementsByServiceCategory(serviceCategoryId);
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _RequredServiceQualificationElementService.GetRequredServiceQualificationElementByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _RequredServiceQualificationElementService.GetRequredServiceQualificationElementById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewRequredServiceQualificationElement(RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceiving)
        {
            return await _RequredServiceQualificationElementService.AddRequredServiceQualificationElement(HttpContext, RequredServiceQualificationElementReceiving);          
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceiving)
        {
            return await _RequredServiceQualificationElementService.UpdateRequredServiceQualificationElement(HttpContext, id, RequredServiceQualificationElementReceiving);
            
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _RequredServiceQualificationElementService.DeleteRequredServiceQualificationElement(id);
        }
    }
}
