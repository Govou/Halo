using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ISbutoQuoteServiceProportionsService
    {
        Task<ApiCommonResponse> GetAllSBUQuoteProportionForQuoteService(long quoteServiceId);
        Task<ApiCommonResponse> SaveSBUToQuoteProp(HttpContext context, IEnumerable<SbutoQuoteServiceProportionReceivingDTO> entities);
    }
}