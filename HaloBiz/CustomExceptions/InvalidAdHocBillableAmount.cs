using System;

namespace HaloBiz.CustomExceptions
{
    [Serializable]
    public class InvalidAdHocBillableAmount : Exception
    {
        public InvalidAdHocBillableAmount(){}

        public InvalidAdHocBillableAmount(string meassage)
            :base(meassage) {}
        
        public InvalidAdHocBillableAmount(string message, Exception inner)
        : base(message, inner) { }
    }
}


