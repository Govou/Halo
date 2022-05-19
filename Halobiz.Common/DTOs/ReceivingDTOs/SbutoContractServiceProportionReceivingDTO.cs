using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class SbutoContractServiceProportionReceivingDTO
    {
        public double Proportion { get; set; }
        public ProportionStatusType Status { get; set; }
        public long UserInvolvedId { get; set; }
        public long ContractServiceId { get; set; }
        public long StrategicBusinessUnitId { get; set; }
    }
}
