using AutoMapper;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.Extensions.Logging;
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
    }
}
