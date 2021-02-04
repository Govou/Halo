using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;

namespace HaloBiz.Model.LAMS
{
    public class TaskFulfillment
    {
        [Key]
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
        public long? ResponsibleId { get; set; }
        public UserProfile Responsible { get; set; }
        public long? AccountableId { get; set; }
        public UserProfile Accountable { get; set; }
        public long? ConsultedId { get; set; }
        public UserProfile Consulted { get; set; }
        public long? InformedId { get; set; }
        public UserProfile Informed { get; set; }
        public long? SupportId { get; set; }
        public UserProfile Support { get; set; }
        public double Budget { get; set; }
        public bool IsPicked { get; set; }
        public DateTime DateTimePicked {get; set;}
        public bool IsAllDeliverableAssigned {get; set;}
        public DateTime TaskCompletionDateTime {get; set;}
        public bool TaskCompletionStatus {get; set;}
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}

