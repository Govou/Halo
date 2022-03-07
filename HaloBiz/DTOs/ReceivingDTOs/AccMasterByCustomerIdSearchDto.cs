using System.Collections.Generic;

namespace halobiz_backend.DTOs.ReceivingDTOs
{
    public class AccMasterByCustomerIdSearchDto
    {
        public long ClientId { get; set; }
        public List<int> Years { get; set; }
    }
}