namespace HaloBiz.Helpers
{
    public class GenericResponse<T>{
        
            public GenericResponse()
            {
            }
            public GenericResponse(T data)
            {
                Succeeded = true;
                Message = string.Empty;
                Errors = null;
                Data = data;
            }
            public T Data { get; set; }
            public bool Succeeded { get; set; }
            public string[] Errors { get; set; }
            public string Message { get; set; }
        }
}