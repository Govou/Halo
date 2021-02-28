using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.RoleManagement
{
    public class Claim
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ClaimEnum ClaimEnum { get; set; }

        public bool IsDeleted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }

    // DO NOT MODIFY.
    public enum ClaimEnum
    {
        BranchManagement = 1,
        DivisionManagement,
        QualificationManagement,
        ServicesManagement,
        SbuManagement,
        TargetManagement,
        BanksManagement,
        SLAManagement,
        LamsManagement,
        ProjectManagement,
        TaskManagement,
        RoleManagement,
        ApprovalsManagement,
        AccountsManagement,
        SetupManagement,
        ClientManagement,
        EndorsementManagement
    }
}
