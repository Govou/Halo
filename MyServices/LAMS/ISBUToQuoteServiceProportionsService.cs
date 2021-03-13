using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ISBUToQuoteServiceProportionsService
    {
        Task<ApiResponse> GetAllSBUQuoteProportionForQuoteService(long quoteServiceId);
        Task<ApiResponse> SaveSBUToQuoteProp(HttpContext context, IEnumerable<SBUToQuoteServiceProportionReceivingDTO> entities);
    }
}