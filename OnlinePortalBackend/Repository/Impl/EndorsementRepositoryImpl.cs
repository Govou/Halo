using AutoMapper;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class EndorsementRepositoryImpl : IEndorsementRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EndorsementRepositoryImpl> _logger;
        private readonly IMapper _mapper;
        public EndorsementRepositoryImpl(HalobizContext context,
            ILogger<EndorsementRepositoryImpl> logger,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ContractServiceForEndorsement>> FindEndorsements(long userId, int limit = 10)
        {
            return _context.ContractServiceForEndorsements.Where(x => x.CreatedById == userId).Take(10);
        }

        public async Task<ContractServiceForEndorsement> FindEndorsementById(long userId, long Id)
        {
            return _context.ContractServiceForEndorsements.FirstOrDefault(x => x.CreatedById == userId && x.Id == Id);
        }

        public async Task<ContractServiceDTO> GetContractService(int id)
        {
            var contractService = _context.ContractServices.Include(x => x.Contract).Include(x => x.Service).FirstOrDefault(x => x.Id == id);
            
            if (contractService == null)
                return null;
            var service = _context.Services.Include(x => x.ServiceType).Include(x => x.ServiceCategory).Include(x => x.AdminRelationship).FirstOrDefault(x => x.Id == contractService.ServiceId);

            var result = new ContractServiceDTO
            {
                ContractServiceId = (int)contractService.Id,
                ServiceName = service.Name,
                ServiceCategory = service.ServiceCategory.Name,
                Quantity = (int)contractService.Quantity,
                ImageUrl = service.ImageUrl,
                Isvatable = service.IsVatable.Value,
                UnitPrice = service.UnitPrice,
                ServiceCode = service.ServiceCode,
                ServiceType = service.ServiceType?.Caption,
                ServiceDescription = service.Description,
                HasDirectComponent = service.AdminRelationship?.DirectServiceId != null && service.AdminRelationship?.AdminService != null ? true : false,
                HasAdminComponent = service.AdminRelationship?.AdminServiceId != null && service.AdminRelationship?.DirectService != null ? true : false,
                TotalContractValue = (int)contractService.Quantity * service.UnitPrice
            };

            return result;
        }

        public async Task<IEnumerable<ContractServiceDTO>> GetContractServices(int userId)
        {
            var contractServiceDTOs = new List<ContractServiceDTO>();
            var contracts = _context.Contracts.Include(x => x.ContractServices).Where(x => x.CustomerDivisionId == userId && !x.IsDeleted && !x.IsDeleted);
            if (contracts == null)
            {
                return null;
            }
            var contractServices = new List<ContractService>();
            foreach (var item in contracts)
            {
                contractServices.AddRange(item.ContractServices);
            }
            var validContractServices = contractServices.Where(x => x.ContractEndDate > DateTime.Today);
            foreach (var contractService in validContractServices)
            {
                var service = _context.Services.Include(x => x.ServiceType).Include(x => x.ServiceCategory).Include(x => x.AdminRelationship).FirstOrDefault(x => x.Id == contractService.ServiceId);

                var result = new ContractServiceDTO
                {
                    ContractServiceId = (int)contractService.Id,
                    ServiceName = service.Name,
                    ServiceCategory = service.ServiceCategory.Name,
                    Quantity = (int)contractService.Quantity,
                    Isvatable = service.IsVatable.Value,
                    ImageUrl = service.ImageUrl,
                    UnitPrice = service.UnitPrice,
                    ServiceCode = service.ServiceCode,
                    ServiceType = service.ServiceType?.Caption,
                    ServiceDescription = service.Description,
                    HasDirectComponent = service.AdminRelationship?.DirectServiceId != null && service.AdminRelationship?.AdminService != null ? true : false,
                    HasAdminComponent = service.AdminRelationship?.AdminServiceId != null && service.AdminRelationship?.DirectService != null ? true : false,
                    TotalContractValue = (int)contractService.Quantity * service.UnitPrice
                };

                contractServiceDTOs.Add(result);
            }
            return contractServiceDTOs;
        }
    }
}
