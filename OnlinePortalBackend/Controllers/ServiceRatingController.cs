using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceRatingController : ControllerBase
    {
        private readonly IServiceRatingService _serviceRatingService;

        public ServiceRatingController(IServiceRatingService serviceRatingService)
        {
            this._serviceRatingService = serviceRatingService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetServiceRatings()
        {
            var response = await _serviceRatingService.FindAllServiceRatings();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceRating = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<ServiceRatingTransferDTO>)serviceRating);
        }

        [HttpGet("GetReviewHistoryByServiceId/{serviceId}")]
        public async Task<ActionResult> GetReviewHistoryByContractServiceId(long contractServiceId)
        {
            var response = await _serviceRatingService.GetReviewHistoryByServiceId(contractServiceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceRating = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<ServiceRatingTransferDTO>)serviceRating);
        }

        [HttpGet("GetMyServiceRatings")]
        public async Task<ActionResult> GetMyServiceRatings()
        {
            var response = await _serviceRatingService.GetMyServiceRatings(HttpContext);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceRating = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<ServiceRatingTransferDTO>)serviceRating);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _serviceRatingService.FindServiceRatingById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceRating = ((ApiOkResponse)response).Result;
            return Ok(serviceRating);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewServiceRating(ServiceRatingReceivingDTO serviceRatingReceiving)
        {
            var response = await _serviceRatingService.AddServiceRating(HttpContext, serviceRatingReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceRating = ((ApiOkResponse)response).Result;
            return Ok((ServiceRatingTransferDTO)serviceRating);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateById(long id, ServiceRatingReceivingDTO serviceRatingReceiving)
        //{
        //    var response = await _serviceRatingService.UpdateServiceRating(HttpContext, id, serviceRatingReceiving);
        //    if (response.StatusCode >= 400)
        //        return StatusCode(response.StatusCode, response);
        //    var serviceRating = ((ApiOkResponse)response).Result;
        //    return Ok((ServiceRatingTransferDTO)serviceRating);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteById(int id)
        //{
        //    var response = await _serviceRatingService.DeleteServiceRating(id);
        //    return StatusCode(response.StatusCode);
        //}
    }
}