using HaloBiz.EnumResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.GenericResponseDTO
{
    public class ApiGenericResponse<T>
    {
        public long responseCode { get; set; }
        public string responseMessage { get; set; }

        public T data { get; set; }
    }
}
