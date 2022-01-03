namespace HaloBiz.DTOs.ApiDTOs
{
    public class ApiResponse
    {
        public int StatusCode { get; }
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return "Resource not found";
                case 200:
                    return "Successful";
                case 411:
                    return "Resource has no Return Route";
                case 409:
                    return "Record already exists";
                case 500:
                    return "An unhandled error occurred";
                case 440:
                    return "Please check your Start or End Date";
                case 441:
                    return "Please check your Opening or Closing Time";
                case 442:
                    return "All Available days can't be false";
                case 443:
                    return "Sorry, this resource is not allowed on this route";
                case 444:
                    return "Sorry, this item is temporarily held for action";
                case 445:
                    return "Sorry, maximum quantity required exceeded";
                case 446:
                    return "Sorry, applicableType is not attached to this service";
                case 447:
                    return "Sorry, this resource has no schedule";
                case 448:
                    return "Sorry, scheduling Date/time and assignment date/time mismatch";

                //case 409:
                //    return "This user cannot be assigned to a different market area or the same service category";
                default:
                    return null;
            }
        }
    }
}