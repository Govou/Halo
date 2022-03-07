using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppReviewController : ControllerBase
    {
        private readonly IAppReviewService _AppReviewService;

        public AppReviewController(IAppReviewService appReviewService)
        {
            this._AppReviewService = appReviewService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAppReview()
        {
            return await _AppReviewService.GetAllAppReview();
        }
        
        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _AppReviewService.GetAppReviewByName(name);
            
                
            var AppReview = ((ApiOkResponse)response).Result;
            return Ok(AppReview);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _AppReviewService.GetAppReviewById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAppReview(AppReviewReceivingDTO AppReviewReceiving)
        {
            return await _AppReviewService.AddAppReview(HttpContext, AppReviewReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, AppReviewReceivingDTO AppReviewReceiving)
        {
            return await _AppReviewService.UpdateAppReview(HttpContext, id, AppReviewReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _AppReviewService.DeleteAppReview(id);
        }
    }
}
