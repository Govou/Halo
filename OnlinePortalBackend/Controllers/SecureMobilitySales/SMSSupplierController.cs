using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.AdapterDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSSupplierController : ControllerBase
    {
        ISupplierService _supplierService;
        public SMSSupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }


       
        [HttpPost("BookAsset")]
        public Task<ApiCommonResponse> BookNewAsset(SupplierBookAssetDTO request)
        {
            return _supplierService.BookAsset(request);
        }

        [HttpPost("AddNewAsset")]
        public Task<ApiCommonResponse> NewAssetAddition(AssetAdditionDTO request)
        {
            return _supplierService.NewAssetAddition(request);
        }

        [HttpGet("GetServiceCenters")]
        public Task<ApiCommonResponse> GetServiceCenters(string state)
        {
            return _supplierService.GetServiceCenters(state);
        }
    }
}
