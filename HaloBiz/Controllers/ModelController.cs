
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    public class ModelController : Controller
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            this._modelService = modelService;
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddModel(ModelReceivingDTO modelReceiving)
        {
            return await _modelService.AddModel(HttpContext, modelReceiving);
        }

        [HttpGet("make/{makeId}")]
        public async Task<ApiCommonResponse> GetAllModelByMake(int makeId)
        {
            return await _modelService.GetAllModelByMake(makeId);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ModelReceivingDTO modelReceiving)
        {
            return await _modelService.UpdateModel(HttpContext, id, modelReceiving);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetModelById(long id)
        {
            return await _modelService.GetModelById(id);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteModel(long id)
        {
            return await _modelService.DeleteModel(id);
        }
    }
}
