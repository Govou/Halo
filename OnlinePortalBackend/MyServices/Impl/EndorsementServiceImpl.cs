using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class EndorsementServiceImpl : IEndorsementService
    {
        private readonly ILogger<EndorsementServiceImpl> _logger;
        private readonly IEndorsementRepository _endorsementRepo;
        private readonly IMapper _mapper;

        public EndorsementServiceImpl(ILogger<EndorsementServiceImpl> logger,
            IEndorsementRepository endorsementRepo,
            IMapper mapper)
        {
            _logger = logger;
            _endorsementRepo = endorsementRepo;
            _mapper = mapper;
        }
        public async Task<ApiCommonResponse> FetchEndorsements(HttpContext context, int limit = 10)
        {
           var endorsements = await _endorsementRepo.FindEndorsements(context.GetLoggedInUserId(), limit);
            if (endorsements.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var endorsementDTOs = _mapper.Map<IEnumerable<EndorsementDTO>>(endorsements);
            return CommonResponse.Send(ResponseCodes.SUCCESS, endorsementDTOs);
        }

        public async Task<ApiCommonResponse> TrackEndorsement(HttpContext context, long endorsementId)
        {
            var endorsement = await _endorsementRepo.FindEndorsementById(context.GetLoggedInUserId(), endorsementId);
            var result = string.Empty;
            if (endorsement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (endorsement.IsDeclined) result = "Declined";
            else if (endorsement.IsApproved) result = "Approved";
            else result = "Pending";
      
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
    }
}
