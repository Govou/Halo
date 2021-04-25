using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class CustomerDivisionServiceImpl : ICustomerDivisionService
    {
        private readonly ILogger<CustomerDivisionServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ICustomerDivisionRepository _CustomerDivisionRepo;
        private readonly IMapper _mapper;
        private readonly ITaskFulfillmentRepository _taskRepo;

        public CustomerDivisionServiceImpl(IModificationHistoryRepository historyRepo, 
                        ICustomerDivisionRepository CustomerDivisionRepo, 
                        ILogger<CustomerDivisionServiceImpl> logger, 
                        IMapper mapper, ITaskFulfillmentRepository taskRepo)
        {
            this._mapper = mapper;
            this._taskRepo = taskRepo;
            this._historyRepo = historyRepo;
            this._CustomerDivisionRepo = CustomerDivisionRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddCustomerDivision(HttpContext context, CustomerDivisionReceivingDTO CustomerDivisionReceivingDTO)
        {
            var CustomerDivision = _mapper.Map<CustomerDivision>(CustomerDivisionReceivingDTO);
            CustomerDivision.CreatedById = context.GetLoggedInUserId();
            var savedCustomerDivision = await _CustomerDivisionRepo.SaveCustomerDivision(CustomerDivision);
            if (savedCustomerDivision == null)
            {
                return new ApiResponse(500);
            }
            var CustomerDivisionTransferDTOs = _mapper.Map<CustomerDivisionTransferDTO>(CustomerDivision);
            return new ApiOkResponse(CustomerDivisionTransferDTOs);
        }

        public async Task<ApiResponse> DeleteCustomerDivision(long id)
        {
            var CustomerDivisionToDelete = await _CustomerDivisionRepo.FindCustomerDivisionById(id);
            if (CustomerDivisionToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _CustomerDivisionRepo.DeleteCustomerDivision(CustomerDivisionToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllCustomerDivisions()
        {
            var CustomerDivisions = await _CustomerDivisionRepo.FindAllCustomerDivision();
            if (CustomerDivisions == null)
            {
                return new ApiResponse(404);
            }
            // var CustomerDivisionTransferDTOs = _mapper.Map<IEnumerable<CustomerDivisionTransferDTO>>(CustomerDivisions);
            return new ApiOkResponse(CustomerDivisions);
        }
        public async Task<ApiResponse> GetCustomerDivisionsByGroupType(long groupTypeId)
        {
            try{
                var clients = await _CustomerDivisionRepo.FindCustomerDivisionsByGroupType(groupTypeId);
                return new ApiOkResponse(clients);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
           
        }

        public async Task<ApiResponse> GetCustomerDivisionByName(string name)
        {
            var CustomerDivision = await _CustomerDivisionRepo.FindCustomerDivisionByName(name);
            if (CustomerDivision == null)
            {
                return new ApiResponse(404);
            }
            var CustomerDivisionTransferDTOs = _mapper.Map<CustomerDivisionTransferDTO>(CustomerDivision);
            return new ApiOkResponse(CustomerDivisionTransferDTOs);
        }

        public async Task<ApiResponse> GetCustomerDivisionBreakDownById(long id)
        {
            try{
                var client = await  _CustomerDivisionRepo.GetCustomerDivisionBreakDownById(id);
                var listOfContractToAmountPaid = await _CustomerDivisionRepo.GetPaymentsPerContractByCustomerDivisionId(id);
                
                var clientTransferDto = _mapper.Map<CustomerDivisionTransferDTO>(client);
                ContractToPaidAmountTransferDTO value = null;
                foreach (var contract in clientTransferDto.Contracts)
                {
                    value = listOfContractToAmountPaid.FirstOrDefault(x => x.ContractId == contract.Id);
                    contract.AmountPaid = value != null ? value.AmountPaid : 0;
                }

                var taskFulfillments = await _taskRepo.GetTaskFulfillmentsByCustomerDivisionId(id);

                clientTransferDto.TaskFulfillments = _mapper.Map<IEnumerable<TaskFulfillmentTransferDTO>>(taskFulfillments);

                return new ApiOkResponse(clientTransferDto);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }


        public async Task<ApiResponse> GetCustomerDivisionById(long id)
        {
            var CustomerDivision = await _CustomerDivisionRepo.FindCustomerDivisionById(id);
            if (CustomerDivision == null)
            {
                return new ApiResponse(404);
            }
            var CustomerDivisionTransferDTOs = _mapper.Map<CustomerDivisionTransferDTO>(CustomerDivision);
            return new ApiOkResponse(CustomerDivisionTransferDTOs);
        }

        public async Task<ApiResponse> GetTaskAndFulfillmentsByCustomerDivisionId(long customerDivisionId)
        {
            var taskAndDeliverables = await _CustomerDivisionRepo.FindTaskAndFulfillmentsByCustomerDivisionId(customerDivisionId);
            if (taskAndDeliverables == null)
            {
                return new ApiResponse(404);
            }
            var taskAndDeliverablesDTOs = _mapper.Map<List<TaskFulfillmentTransferDTO>>(taskAndDeliverables);
            return new ApiOkResponse(taskAndDeliverablesDTOs);
        }

        public async Task<ApiResponse> UpdateCustomerDivision(HttpContext context, long id, CustomerDivisionReceivingDTO CustomerDivisionReceivingDTO)
        {
            var CustomerDivisionToUpdate = await _CustomerDivisionRepo.FindCustomerDivisionById(id);
            if (CustomerDivisionToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {CustomerDivisionToUpdate.ToString()} \n";
            CustomerDivisionToUpdate.DivisionName = CustomerDivisionReceivingDTO.DivisionName;
            CustomerDivisionToUpdate.CustomerId = CustomerDivisionReceivingDTO.CustomerId;
            CustomerDivisionToUpdate.PhoneNumber = CustomerDivisionReceivingDTO.PhoneNumber;
            CustomerDivisionToUpdate.Rcnumber = CustomerDivisionReceivingDTO.RCNumber;
            CustomerDivisionToUpdate.Email = CustomerDivisionReceivingDTO.Email;
            CustomerDivisionToUpdate.Industry = CustomerDivisionReceivingDTO.Industry;
            CustomerDivisionToUpdate.LogoUrl = CustomerDivisionReceivingDTO.LogoUrl;
            var updatedCustomerDivision = await _CustomerDivisionRepo.UpdateCustomerDivision(CustomerDivisionToUpdate);
            summary += $"Details after change, \n {CustomerDivisionToUpdate.ToString()} \n";

            if (updatedCustomerDivision == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "CustomerDivision",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedCustomerDivision.Id
            };

            await _historyRepo.SaveHistory(history);
            var CustomerDivisionTransferDTOs = _mapper.Map<CustomerDivisionTransferDTO>(updatedCustomerDivision);
            return new ApiOkResponse(CustomerDivisionTransferDTOs);


        }

        public async Task<ApiResponse> GetClientsWithSecuredMobilityContractServices()
        {
            var CustomerDivisions = await _CustomerDivisionRepo.GetClientsWithSecuredMobilityContractServices();
            if (CustomerDivisions == null)
            {
                return new ApiResponse(404);
            }
            var CustomerDivisionTransferDTOs = _mapper.Map<IEnumerable<CustomerDivisionTransferDTO>>(CustomerDivisions);
            return new ApiOkResponse(CustomerDivisionTransferDTOs);
        }
    }
}
