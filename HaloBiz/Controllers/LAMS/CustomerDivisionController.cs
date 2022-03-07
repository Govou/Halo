using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
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
        public async Task<ApiCommonResponse> GetCustomerDivisions()
        {
            return await _CustomerDivisionService.GetAllCustomerDivisions();
        }

        [HttpGet("GetCustomerDivisionsByGroupType/{groupTypeId}")]
        public async Task<ApiCommonResponse> GetCustomerDivisionsByGroupType(long groupTypeId)
        {
            return await _CustomerDivisionService.GetCustomerDivisionsByGroupType(groupTypeId);
        }


        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _CustomerDivisionService.GetCustomerDivisionById(id);
        }

        [HttpGet("TaskAndDeliverables/{customerDivisionId}")]
        public async Task<ApiCommonResponse> GetTaskAndDeliverables(long customerDivisionId)
        {
            return await _CustomerDivisionService.GetTaskAndFulfillmentsByCustomerDivisionId(customerDivisionId);
        }

        [HttpGet("GetClientsWithSecuredMobilityContractService")]
        public async Task<ApiCommonResponse> GetClientsWithSecuredMobilityContractService()
        {
            return await _CustomerDivisionService.GetClientsWithSecuredMobilityContractServices();
        }

        [HttpGet("GetClientsUnAssignedToRMSbu")]
        public async Task<ApiCommonResponse> GetClientsUnAssignedToRMSbu()
        {
            return await _CustomerDivisionService.GetClientsUnAssignedToRMSbu();
        }

        [HttpGet("GetClientsAttachedToRMSbu/{sbuId}")]
        public async Task<ApiCommonResponse> GetClientsAttachedToRMSbu(long sbuId)
        {
            return await _CustomerDivisionService.GetClientsAttachedToRMSbu(sbuId);
        }

        [HttpGet("GetRMSbuClientsByGroupType/{sbuId}/{clientTypeId}")]
        public async Task<ApiCommonResponse> GetRMSbuClientsByGroupType(long sbuId, long clientTypeId)
        {
            return await _CustomerDivisionService.GetRMSbuClientsByGroupType(sbuId, clientTypeId);
        }

        [HttpPut("AttachClientToRMSbu/{customerDivisionId}/{sbuId}")]
        public async Task<ApiCommonResponse> AttachClientToRMSbu(long customerDivisionId, long sbuId)
        {
            return await _CustomerDivisionService.AttachClientToRMSbu(HttpContext, customerDivisionId, sbuId);
        }

        [HttpGet("ContractsBreakDown/{customerDivsionId}")]
        public async Task<ApiCommonResponse> GetContractBreakDownId(long customerDivsionId)
        {
            return await _CustomerDivisionService. GetCustomerDivisionBreakDownById(customerDivsionId);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewCustomerDivision(CustomerDivisionReceivingDTO CustomerDivisionReceiving)
        {
            return await _CustomerDivisionService.AddCustomerDivision(HttpContext, CustomerDivisionReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, CustomerDivisionReceivingDTO CustomerDivisionReceiving)
        {
            return await _CustomerDivisionService.UpdateCustomerDivision(HttpContext, id, CustomerDivisionReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _CustomerDivisionService.DeleteCustomerDivision(id);
        }

        [HttpGet("GetByCustomerNumber/{dTrackCustomerNumber}")]
        public async Task<ApiCommonResponse> GetByDTrackCustomerNumber(string dTrackCustomerNumber)
        {
            return await _CustomerDivisionService.GetCustomerDivisionByDTrackCustomerNumber(dTrackCustomerNumber);
        }
    }
}