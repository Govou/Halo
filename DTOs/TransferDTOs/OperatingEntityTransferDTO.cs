using HaloBiz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class OperatingEntityTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; } //For holding Cost Center Code (Dtrack)
        public  UserProfileTransferDTO Head { get; set; }
        public long DivisionId { get; set; }
        public SBUProportionTransferDTO SBUProportion { get; set; }
        public DivisionWithoutOperatingEntityDTO Division { get; set; }
        public IEnumerable<ServiceGroupTransferDTO> ServiceGroups { get; set; }
        public IEnumerable<SBUWithoutOperatingEntityTransferDTO> StrategicBusinessUnits { get; set; }
    }
}
