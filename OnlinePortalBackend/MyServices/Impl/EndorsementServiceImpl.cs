using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class EndorsementServiceImpl : IEndorsementService
    {
        private readonly ILogger<EndorsementServiceImpl> _logger;
        private readonly IEndorsementRepository _endorsementRepo;
        private readonly IMapper _mapper;

        public EndorsementServiceImpl(ILogger<CartContractServiceImpl> logger,
            IEndorsementRepository endorsementRepo,
            IMapper mapper)
        {
            _logger = logger;
            _endorsementRepo = endorsementRepo;
            _mapper = mapper;
        }
        public Task<string> FetchEndorsements(HttpContext context, int limit = 10)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> TrackEndorsement(HttpContext context, string endorsementId)
        {
            throw new System.NotImplementedException();
        }
    }
}
