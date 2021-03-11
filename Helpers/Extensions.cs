using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

        }

        public static long GetLoggedInUserId(this HttpContext context)
        {
            return long.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out long userIdClaim) ?
                userIdClaim : 31;

        }

        public static IEnumerable<RequiredServiceDocumentTransferDTO> GetListOfRequiredDocuments(this IEnumerable<ServiceRequiredServiceDocument> docs)
        {
            var reqDocs = new List<RequiredServiceDocumentTransferDTO>();
            foreach (var item in docs)
            {
                reqDocs.Add(new RequiredServiceDocumentTransferDTO(){
                    Caption = item.RequiredServiceDocument.Caption,
                    Description = item.RequiredServiceDocument.Description,
                    Id = item.RequiredServiceDocument.Id,
                    Type = item.RequiredServiceDocument.Type
                });
            }

            return reqDocs;

        }
        public static IEnumerable<RequredServiceQualificationElementTransferDTO> GetListOfRequiredQualificationElements(this IEnumerable<ServiceRequredServiceQualificationElement> elements)
        {
            var reqElements = new List<RequredServiceQualificationElementTransferDTO>();
            foreach (var item in elements)
            {
                reqElements.Add(new RequredServiceQualificationElementTransferDTO(){
                    Caption = item.RequredServiceQualificationElement.Caption,
                    Description = item.RequredServiceQualificationElement.Description,
                    Id = item.RequredServiceQualificationElement.Id,
                    Type = item.RequredServiceQualificationElement.Type,
                    ServiceCategoryId = item.RequredServiceQualificationElement.ServiceCategoryId
                });
            }
            return reqElements;
        }

        public static string GenerateReferenceNumber(this long refNumber)
        {
            return "HALO" + refNumber.ToString().PadLeft(10, '0');
        }

        public static bool IsSuperAdmin(this UserProfileReceivingDTO userProfile)
        {
            return userProfile.Email.Contains("developer") && 
               (userProfile.Email.Trim().EndsWith("halogen-group.com") ||
                userProfile.Email.Trim().EndsWith("avanthalogen.com") ||
                userProfile.Email.Trim().EndsWith("averthalogen.com") ||
                userProfile.Email.Trim().EndsWith("armourxhalogen.com") ||
                userProfile.Email.Trim().EndsWith("pshalogen.com") ||
                userProfile.Email.Trim().EndsWith("academyhalogen.com") ||
                userProfile.Email.Trim().EndsWith("armadahalogen.com")
              );
        }

        public static double GetTotalContractValue(this IEnumerable<ContractService> contractServices)
        {
            return contractServices.Sum( x => x.GetNumberValuePerContractService());
        }

        public static double GetNumberValuePerContractService(this ContractService contractService)
        {
            if(contractService.InvoicingInterval == TimeCycle.OneTime){
                return contractService.BillableAmount?? 0.0;
            }
            var startDate = (DateTime) contractService.ContractStartDate;
            var totalAmount = 0.0;
            while(startDate < contractService.ContractEndDate)
            {
                totalAmount += contractService.BillableAmount?? 0.0;
                startDate = startDate.AddMonths(1);
            }

            return totalAmount;
        }

        public static void RunAsTask(this Action action)
        {
            Task.Run(action);
        }
    }
}