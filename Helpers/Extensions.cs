using System;
using System.Collections.Generic;
using System.Security.Claims;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
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
                    Id = item.RequiredServiceDocument.Id
                });
            }

            return reqDocs;

        }
    }
}