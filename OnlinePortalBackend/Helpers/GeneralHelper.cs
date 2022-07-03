using System;

namespace OnlinePortalBackend.Helpers
{
    public class GeneralHelper
    {
        public static int GetPaymentGateway(string gateway)
        {
            var result = 0;
            if (gateway.ToLower().Contains("flutter"))
            {
                result = 1;
            }
            if (gateway.ToLower().Contains("paystack"))
            {
                result = 2;
            }
           
            return result;
        }

        public static object ConvertDateToLongString(DateTime dateTime)
        {
            var dateStr = dateTime.ToString("dd/MM/yyyy/HH/mm/ss");
            dateStr = dateStr.Replace("/", "");
            return dateStr;
        }

    }
}
