using HaloBiz.EnumResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.GenericResponseDTO
{
    public class ApiGenericResponse
    {
        public EnumsResponse responseCode { get; set; }
        //public T responseDesc { get; set; }
    }
}
