using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.GenericResponseDTO
{
    public class ResponseMessage
    {
        public const string ENTITYNOTSAVED = "The entity could not be saved";
        public const string EntitySuccessfullySaved = "The entity was saved successfully.";
        public const string EntityNotFound  = "The entity was not successfully found.";
        public const string EntitySuccessfullyFound = "the entity was successfully found";

    }
}
