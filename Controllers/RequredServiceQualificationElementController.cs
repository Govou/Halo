using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RequredServiceQualificationElementController : ControllerBase
    {
        private readonly IRequredServiceQualificationElementService _RequredServiceQualificationElementService;

        public RequredServiceQualificationElementController(IRequredServiceQualificationElementService RequredServiceQualificationElementService)
        {
            this._RequredServiceQualificationElementService = RequredServiceQualificationElementService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetRequredServiceQualificationElement()
        {
            var response = await _RequredServiceQualificationElementService.GetAllRequredServiceQualificationElements();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var RequredServiceQualificationElement = ((ApiOkResponse)response).Result;
            return Ok(RequredServiceQualificationElement);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _RequredServiceQualificationElementService.GetRequredServiceQualificationElementByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var RequredServiceQualificationElement = ((ApiOkResponse)response).Result;
            return Ok(RequredServiceQualificationElement);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _RequredServiceQualificationElementService.GetRequredServiceQualificationElementById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var RequredServiceQualificationElement = ((ApiOkResponse)response).Result;
            return Ok(RequredServiceQualificationElement);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewRequredServiceQualificationElement(RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceiving)
        {
            var response = await _RequredServiceQualificationElementService.AddRequredServiceQualificationElement(HttpContext, RequredServiceQualificationElementReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var RequredServiceQualificationElement = ((ApiOkResponse)response).Result;
            return Ok(RequredServiceQualificationElement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceiving)
        {
            var response = await _RequredServiceQualificationElementService.UpdateRequredServiceQualificationElement(HttpContext, id, RequredServiceQualificationElementReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var RequredServiceQualificationElement = ((ApiOkResponse)response).Result;
            return Ok(RequredServiceQualificationElement);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _RequredServiceQualificationElementService.DeleteRequredServiceQualificationElement(id);
            return StatusCode(response.StatusCode);
        }
    }
}
