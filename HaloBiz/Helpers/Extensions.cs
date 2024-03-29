using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.Helpers;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
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
            var name = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var id =  long.TryParse(name, out long userIdClaim) ?
                userIdClaim : 0;
            if (id == 0)
                throw new Exception("The user is not logged in");
            return id;
        }

        public static string GetLoggedInUserEmail(this HttpContext context)
        {
            try
            {
                string userEmailClaim = context.User.FindFirstValue(ClaimTypes.Email);
                return !String.IsNullOrWhiteSpace(userEmailClaim) ? userEmailClaim : "itsupport@halogen-group.com";
            }
            catch { return "itsupport@halogen-group.com"; }
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
            return (userProfile.Email.Equals("ahmed.sulaiman@halogen-group.com") ||
                    userProfile.Email.Equals("omoshola.yusuf@halogen-group.com"))
                && 
               (userProfile.Email.Trim().EndsWith("halogen-group.com") ||
                userProfile.Email.Trim().EndsWith("avanthalogen.com") ||
                userProfile.Email.Trim().EndsWith("averthalogen.com") ||
                userProfile.Email.Trim().EndsWith("armourxhalogen.com") ||
                userProfile.Email.Trim().EndsWith("pshalogen.com") ||
                userProfile.Email.Trim().EndsWith("academyhalogen.com") ||
                userProfile.Email.Trim().EndsWith("armadahalogen.com"));
        }

        public static double GetTotalContractValue(this IEnumerable<ContractService> contractService)
        {
            return contractService.Sum( x => x.GetNumberValuePerContractService());
        }

        public static double GetNumberValuePerContractService(this ContractService contractService)
        {
            if(contractService.InvoicingInterval == (int)TimeCycle.OneTime){
                return contractService.BillableAmount ?? 0.0;
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

        public static string GetStateShortName(string stateName)
        {
            switch (stateName?.ToUpper())
            {
                case "ABA":
                    return "ABA";
                case "ABUJA":
                    return "ABJ";
                case "ABEOKUTA":
                    return "ABK";
                case "ASABA":
                    return "ASA";
                case "BAUCHI":
                    return "BCH";
                case "BENIN":
                    return "BEN";
                case "BIRNIN-KEBBI":
                    return "BKB";
                case "CALABAR":
                    return "CAL";
                case "ENUGU":
                    return "ENU";
                case "GUSAU":
                    return "GUA";
                case "IBADAN":
                    return "IBA";
                case "ILORIN":
                    return "ILR";
                case "JIGAWA":
                    return "JGW";
                case "JOS":
                    return "JOS";
                case "KADUNA":
                    return "KD";
                case "KANO":
                    return "KN";
                case "KASTINA":
                    return "KST";
                case "LAGOS":
                    return "LA";
                case "MAIDUGURI":
                    return "MDG";
                case "MINNA":
                    return "MIN";
                case "ONITSHA":
                    return "ONT";
                case "OSOGBO":
                    return "OSO";
                case "OWERRI":
                    return "OWR";
                case "PORTHARCOURT":
                    return "PH";
                case "SOKOTO":
                    return "SKT";
                default:
                    return "LA"; //Lagos default to avoid errors //stateName?.ToUpper()?.Substring(0, 4);
            }
        }

        public static string GetIndustryShortName(string industry)
        {
            switch (industry?.ToUpper())
            {
                case "SERVICE INDUSTRY":
                    return "01";
                case "MANUFACTURING":
                    return "02";
                case "TELECOMMUNICATIONS":
                    return "03";
                case "OIL AND GAS INDUSTRY":
                    return "04";
                case "FINANCIAL SECTOR":
                    return "05";
                case "INDIVIDUAL":
                    return "06";
                case "PUBLIC SECTOR":
                    return "07";
                case "OTHERS":
                    return "08";
                case "UNIFORMS":
                    return "09";
                case "BOOTS AND OTHERS":
                    return "10";
                case "STATIONERIES":
                    return "11";
                case "COMPUTER ASSESSORIES":
                    return "12";
                case "COMPUTER HARDWARE":
                    return "13";
                case "FURNITURE":
                    return "14";
                case "OFFICE EQUIPMENT":
                    return "15";
                case "MAINTENANCE":
                    return "16";
                case "TECHNOLOGY ACCESSORIES":
                    return "17";
                case "INTERNET SUBCRIPTION":
                    return "18";
                case "COMMUNICATION":
                    return "19";
                case "MEDICAL":
                    return "20";
                case "WALKIE TALKIE":
                    return "21";
                case "GENERAL BUSINESS":
                    return "XXX";
                default:
                    return "08"; //OTHERS
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

    }
}