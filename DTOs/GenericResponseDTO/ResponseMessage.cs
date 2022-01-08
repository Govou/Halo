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

        //SecuredMobility
        public const string NotFound404 = "Resource not found.";
        public const string Success200 = "Operation Successful.";
        public const string NoReturnRoute411 = "Resource has no Return Route";
        public const string RecordExists409 = "Record already exists.";
        public const string InternalServer500 = "An unhandled error occurred.";
        public const string StartOrEndDate440 = "Please check your Start or End Date";
        public const string TimeCheck441 = "Please check your Opening or Closing Time";
        public const string DaysAvailability442 = "All Available days can't be false";
        public const string NoResourceOnRoute443 = "Sorry, this resource is not allowed on this route";
        public const string Held444 = "Sorry, this item is temporarily held for action";
        public const string MaxQuantity445 = "Sorry, maximum quantity required exceeded";
        public const string NoApplicableType446 = "Sorry, applicableType is not attached to this service";
        public const string NoSchedule447 = "Sorry, this resource has no schedule";
        public const string ScheduleTimeMismatch448 = "Sorry, scheduling Date/time and assignment date/time mismatch";


    }
}
