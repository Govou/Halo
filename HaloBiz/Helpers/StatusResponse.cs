using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Helpers
{

    public enum GenericStatusMessage
    {
        Success,
        Error
    };
    public class StatusResponse
    {
        public dynamic data { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public int statusCode { get; set; }


        public static StatusResponse SuccessMessage(string message = "", dynamic data = null, bool status = true)
        {
            return new StatusResponse
            {
                data = data,
                message = message ?? GenericStatusMessage.Success.ToString(),
                status = status

            };
        }

        public static StatusResponse ErrorMessage(string message = "", int code = 0)
        {
            return new StatusResponse
            {
                message = message ?? GenericStatusMessage.Error.ToString(),
                status = false

            };
        }

        public static StatusResponse SystemErrMessage( int code = 0)
        {
            return new StatusResponse
            {
                message = "Connectivity Issue, Please try again later",
                status = false
            };
        }
    }
}
