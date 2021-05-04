using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerDivisionController : ControllerBase
    {
        private readonly ICustomerDivisionService _CustomerDivisionService;

        public CustomerDivisionController(ICustomerDivisionService CustomerDivisionService)
        {
            this._CustomerDivisionService = CustomerDivisionService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetCustomerDivisions()
        {
            var response = await _CustomerDivisionService.GetAllCustomerDivisions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("GetCustomerDivisionsByGroupType/{groupTypeId}")]
        public async Task<ActionResult> GetCustomerDivisionsByGroupType(long groupTypeId)
        {
            var response = await _CustomerDivisionService.GetCustomerDivisionsByGroupType(groupTypeId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _CustomerDivisionService.GetCustomerDivisionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("TaskAndDeliverables/{customerDivisionId}")]
        public async Task<ActionResult> GetTaskAndDeliverables(long customerDivisionId)
        {
            var response = await _CustomerDivisionService.GetTaskAndFulfillmentsByCustomerDivisionId(customerDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("GetClientsWithSecuredMobilityContractService")]
        public async Task<ActionResult> GetClientsWithSecuredMobilityContractService()
        {
            var response = await _CustomerDivisionService.GetClientsWithSecuredMobilityContractServices();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("GetClientsUnAssignedToRMSbu")]
        public async Task<ActionResult> GetClientsUnAssignedToRMSbu()
        {
            var response = await _CustomerDivisionService.GetClientsUnAssignedToRMSbu();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("GetClientsAttachedToRMSbu/{sbuId}")]
        public async Task<ActionResult> GetClientsAttachedToRMSbu(long sbuId)
        {
            var response = await _CustomerDivisionService.GetClientsAttachedToRMSbu(sbuId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("GetRMSbuClientsByGroupType/{sbuId}/{clientTypeId}")]
        public async Task<ActionResult> GetRMSbuClientsByGroupType(long sbuId, long clientTypeId)
        {
            var response = await _CustomerDivisionService.GetRMSbuClientsByGroupType(sbuId, clientTypeId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpPut("AttachClientToRMSbu/{customerDivisionId}/{sbuId}")]
        public async Task<ActionResult> AttachClientToRMSbu(long customerDivisionId, long sbuId)
        {
            var response = await _CustomerDivisionService.AttachClientToRMSbu(HttpContext, customerDivisionId, sbuId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpGet("ContractsBreakDown/{customerDivsionId}")]
        public async Task<ActionResult> GetContractBreakDownId(long customerDivsionId)
        {
            var response = await _CustomerDivisionService. GetCustomerDivisionBreakDownById(customerDivsionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewCustomerDivision(CustomerDivisionReceivingDTO CustomerDivisionReceiving)
        {
            var response = await _CustomerDivisionService.AddCustomerDivision(HttpContext, CustomerDivisionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, CustomerDivisionReceivingDTO CustomerDivisionReceiving)
        {
            var response = await _CustomerDivisionService.UpdateCustomerDivision(HttpContext, id, CustomerDivisionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var CustomerDivision = ((ApiOkResponse)response).Result;
            return Ok(CustomerDivision);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _CustomerDivisionService.DeleteCustomerDivision(id);
            return StatusCode(response.StatusCode);
        }
    }
}