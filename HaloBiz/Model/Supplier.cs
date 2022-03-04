using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class Supplier
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(20), MinLength(2), RegularExpression("[\\w\\s\\W]{2,20}")]
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public long? SupplierCategoryId { get; set; }
        public SupplierCategory SupplierCategory { get; set; }
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage="Invalid Email Address")]
        public string SupplierEmail { get; set; }
        [RegularExpression("\\d{10,15}")]
        public string MobileNumber { get; set; }
        public long StateId { get; set; }
        public State State { get; set; }
        public long LGAId { get; set; }
        public LGA LGA { get; set; }
        public string Street { get; set; }
        public string Address { get; set; } //concat
        public string ImageUrl { get; set; } //NC
        public string PrimaryContactName { get; set; } 
        public string PrimaryContactEmail { get; set; } 
        public string PrimaryContactMobile { get; set; } 
        public string PrimaryContactGender { get; set; } //mr, mrs
        //subject to review
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}