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
                //case 409:
                //    return "This user cannot be assigned to a different market area or the same service category";
                default:
                    return null;
            }
        }
    }
}