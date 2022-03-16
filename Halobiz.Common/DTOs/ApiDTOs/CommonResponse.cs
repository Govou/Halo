using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ApiDTOs
{
    public class ApiCommonResponse
    {
        public string responseCode { get; set; }
        public object responseData { get; set; }
        public string responseMsg { get; set; }
    }

    public enum ResponseCodes : short
    {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS = 00,
        /// <summary>
        /// Failure
        /// </summary>
        NO_DATA_AVAILABLE = 01,

        /// <summary>
        /// Some failure of some sort
        /// </summary>
        FAILURE = 02,
        SYSTEM_ERROR_OCCURRED = 03,
        NO_USER_PROFILE_FOUND = 04,
        INVALID_LOGIN_DETAILS = 05,
        UNAUTHORIZED = 06,
        EMAIL_NOT_EXIST = 07,
        DUPLICATE_REQUEST = 08,
    }

    public static class CommonResponse
    {
        public static ApiCommonResponse Send(ResponseCodes code, object payload =  null, string message = "")
        {
            return new ApiCommonResponse
            {
                responseCode = ((int)code).ToString().Length == 1 ? $"0{(int)code}" : ((int)code).ToString(),
                responseData = payload,
                responseMsg = string.IsNullOrEmpty(message) ? code.ToString() : message
            };
        }
    }   

    
}
