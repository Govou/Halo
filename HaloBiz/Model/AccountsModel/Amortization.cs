using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Model.AccountsModel
{
    public class Amortization
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long ClientId { get; set; }
        public Customer Client { get; set; }
        [Required]
        public long DivisionId { get; set; }
        public CustomerDivision Division { get; set; }
        [Required]
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        [Required]
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
        [Required]
        public double ContractValue { get; set; }
        [Required]
        public int Year { get; set; }
        public double January { get; set; } = 0;
        public double February { get; set; } = 0;
        public double March { get; set; } = 0;
        public double April { get; set; } = 0;
        public double May { get; set; } = 0;
        public double June { get; set; } = 0;
        public double July { get; set; } = 0;
        public double August { get; set; } = 0;
        public double September { get; set; } = 0;
        public double October {get; set;} = 0;
        public double November {get; set;} = 0;
        public double December {get; set;} = 0;
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}

