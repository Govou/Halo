using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class CustomerServiceImpl : ICustomerService
    {
        private readonly ILogger<CustomerServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IMapper _mapper;

        public CustomerServiceImpl(IModificationHistoryRepository historyRepo, ICustomerRepository CustomerRepo, ILogger<CustomerServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._CustomerRepo = CustomerRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddCustomer(HttpContext context, CustomerReceivingDTO CustomerReceivingDTO)
        {
            var customer = _mapper.Map<Customer>(CustomerReceivingDTO);
            customer.CreatedById = context.GetLoggedInUserId();
            var savedCustomer = await _CustomerRepo.SaveCustomer(customer);
            if (savedCustomer == null)
            {
                return new ApiResponse(500);
            }
            var CustomerTransferDTOs = _mapper.Map<CustomerTransferDTO>(customer);
            return new ApiOkResponse(CustomerTransferDTOs);
        }

        public async Task<ApiResponse> DeleteCustomer(long id)
        {
            var customerToDelete = await _CustomerRepo.FindCustomerById(id);
            if (customerToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _CustomerRepo.DeleteCustomer(customerToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllCustomers()
        {
            var Customers = await _CustomerRepo.FindAllCustomer();
            if (Customers == null)
            {
                return new ApiResponse(404);
            }
            var CustomerTransferDTOs = _mapper.Map<IEnumerable<CustomerTransferDTO>>(Customers);
            return new ApiOkResponse(CustomerTransferDTOs);
        }

        public async Task<ApiResponse> GetCustomersByGroupType(long groupTypeId)
        {
            try{
                var customers = await _CustomerRepo.FindCustomersByGroupType(groupTypeId);
                return new ApiOkResponse(customers); 
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }

        public async Task<ApiResponse> GetCustomerByName(string name)
        {
            var Customer = await _CustomerRepo.FindCustomerByName(name);
            if (Customer == null)
            {
                return new ApiResponse(404);
            }
            var CustomerTransferDTOs = _mapper.Map<CustomerTransferDTO>(Customer);
            return new ApiOkResponse(CustomerTransferDTOs);
        }

        public async Task<ApiResponse> GetCustomerById(long id)
        {
            var Customer = await _CustomerRepo.FindCustomerById(id);
            if (Customer == null)
            {
                return new ApiResponse(404);
            }
            var CustomerTransferDTOs = _mapper.Map<CustomerTransferDTO>(Customer);
            return new ApiOkResponse(CustomerTransferDTOs);
        }
        public async Task<ApiResponse> UpdateCustomer(HttpContext context, long id, CustomerReceivingDTO CustomerReceivingDTO)
        {
            var CustomerToUpdate = await _CustomerRepo.FindCustomerById(id);
            if (CustomerToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {CustomerToUpdate.ToString()} \n";
            CustomerToUpdate.GroupName = CustomerReceivingDTO.GroupName;
            CustomerToUpdate.GroupTypeId = CustomerReceivingDTO.GroupTypeId;
            CustomerToUpdate.PhoneNumber = CustomerReceivingDTO.PhoneNumber;
            CustomerToUpdate.RCNumber = CustomerReceivingDTO.RCNumber;
            CustomerToUpdate.Email = CustomerReceivingDTO.Email;
            var updatedCustomer = await _CustomerRepo.UpdateCustomer(CustomerToUpdate);
            summary += $"Details after change, \n {CustomerToUpdate.ToString()} \n";

            if (updatedCustomer == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Customer",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedCustomer.Id
            };

            await _historyRepo.SaveHistory(history);
            var CustomerTransferDTOs = _mapper.Map<CustomerTransferDTO>(updatedCustomer);
            return new ApiOkResponse(CustomerTransferDTOs);


        }
    }
}
