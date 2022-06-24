using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models.OnlinePortal;
using Newtonsoft.Json;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.DTOs.AdapterDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepo;
        private readonly IPaymentAdapter _adapter;
        public SupplierService(ISupplierRepository supplierRepo, IPaymentAdapter adapter)
        {
            _supplierRepo = supplierRepo;
            _adapter = adapter;
        }

        //public async Task<ApiCommonResponse> BookAsset(SupplierBookAssetDTO request)
        //{
        //    var result = await _adapter.VerifyPaymentAsync((PaymentGateway)GeneralHelper.GetPaymentGateway(request.PaymentGateway), request.PaymentReference);

        //    if (!result.PaymentSuccessful)
        //    {
        //         return CommonResponse.Send(ResponseCodes.FAILURE, null, "Payment could not be verified");
        //    }

        //    var response = await BookAssetOnThridPartyService(request);

        //    if (response.isSuccess)
        //    {
        //        return CommonResponse.Send(ResponseCodes.SUCCESS, null, response.message);
        //    }

        //    return CommonResponse.Send(ResponseCodes.FAILURE, null, response.message);
        //}


      


        public async Task<ApiCommonResponse> GetServiceCenters(string state)
        {
            var serviceCenters = new ServiceInspectionDTO();
            serviceCenters.Data = new List<InspectionDetail>();
            var serviceDetails = GetServiceDetails();

            if (serviceDetails.Status != "success")
                return null;

            if (serviceDetails.Data == null)
                return null;

            foreach (var item in serviceDetails?.Data)
            {
                var result = GetInspectionDetails(state, item?.Description);

                if (result.Status == "success" && result?.Data.Count > 0)
                {
                    serviceCenters.Data.AddRange(result?.Data);
                }
            }

            if (serviceCenters?.Data?.Count > 0)
            {
                serviceCenters.Status = "success";
                serviceCenters.ResponseCode = "00";
            }
            else
            {
                serviceCenters.ResponseCode = "02";
                serviceCenters.Status = "failed";
            }


            if (serviceCenters.ResponseCode == "00")
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, serviceCenters.Data, serviceCenters.Status);
            }
            return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, serviceCenters.Data, serviceCenters.Status);
        }

        private SupplierServiceDetails GetServiceDetails()
        {
            var client = new RestClient("https://api.myvivcar.com/api/services/67");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            var serviceDetail = JsonConvert.DeserializeObject<SupplierServiceDetails>(response.Content);

            return serviceDetail;
        }

        private ServiceInspectionDTO GetInspectionDetails(string state, string serviceName)
        {
            var client = new RestClient("https://api.myvivcar.com/api/services/list");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = new ServiceInspectionRequestDTO
            {
                GroupId = "67",
                State = state,
                ServiceName = serviceName
            };
            var jBody = JsonConvert.SerializeObject(body);

            request.AddParameter("application/json", jBody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
           
            var result = JsonConvert.DeserializeObject<ServiceInspectionDTO>(response.Content);

            return result;
        }

        //private Task<bool> PostTransactionToAccounts(PostTransactionDTO request)
        //{
           
        //}

        public async Task<ApiCommonResponse> NewAssetAddition(AssetAdditionDTO request)
        {
           var result = await _supplierRepo.AddNewAsset(request);

            if (!result)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "failed");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, result, "success");

        }

        public async Task<ApiCommonResponse> GetVehicleMakes()
        {
            var result = await _supplierRepo.GetVehicleMakes();

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "failed");
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result, "success");
        }

        public async Task<ApiCommonResponse> GetVehicleModels(int makeId)
        {
            var result = await _supplierRepo.GetVehicleModels(makeId);

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "failed");
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result, "success");
        }

        public async Task<ApiCommonResponse> GetSupplierCategories()
        {
            var result = await _supplierRepo.GetSupplierCategories();

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "failed");
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result, "success");
        }

        public  async Task<ApiCommonResponse> GetDashboardDetails(int profileId)
        {
            var result = await _supplierRepo.GetDashboardDetails(profileId);

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "failed");
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result, "success");
        }
    }
}
