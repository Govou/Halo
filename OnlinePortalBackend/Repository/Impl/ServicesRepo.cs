using AutoMapper;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class ServicesRepo : IServicesRepo
    {
       
        private readonly HalobizContext _context;
        private readonly ILogger<ServicesRepo> _logger;
        private readonly IMapper _mapper;
        public ServicesRepo(HalobizContext context,
            ILogger<ServicesRepo> logger,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceDetailDTO> GetContractServciceById(int contractServciceId)
        {

            var contractDetail = _context.ContractServices.Where(x => x.Id == contractServciceId).Select(x => new
            {
                Quantity = x.Quantity,
                BillableAmount = x.BillableAmount,
                ContractId = x.ContractId,
                ServiceId = x.ServiceId
            }).FirstOrDefault();

            if (contractDetail == null)
                return null;

           var serviceDetail = _context.Services.Include(a => a.AdminRelationship).Include(d => d.DirectRelationship)
                .Include(sc => sc.ServiceCategory).Include(e => e.ContractServiceForEndorsements).Include(st => st.ServiceType).Include(c => c.ContractServiceForEndorsements).Where(x => x.Id == contractDetail.ServiceId).Select(x =>
                new ServiceDetailDTO
                {
                    Category = x.ServiceCategory.Name,
                    Code = x.ServiceCode,
                    Description = x.Description,
                    Name = x.Name,
                    UnitPrice = x.UnitPrice,
                    HasAdminComponent = x.AdminRelationship.AdminServiceId == null ? false : true,
                    HasDirectServiceComponent = x.DirectRelationship.DirectServiceId == null ? false : true,
                    IsVatable = x.IsVatable.Value,
                    Type = x.ServiceType.Caption,
                    EndorsementHistory = x.ContractServiceForEndorsements.Where(x => x.Id == contractDetail.ContractId).Select(x => new EndorsementHistory { Date = x.CreatedAt, Description = x.EndorsementDescription, Type = x.EndorsementType.Caption}),
                    Quantity = (int)contractDetail.Quantity,
                    TotalContractValue = contractDetail.BillableAmount.Value
                }
            );
            return serviceDetail.FirstOrDefault();

        }
    }
}
