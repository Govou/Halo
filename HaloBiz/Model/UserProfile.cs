using HaloBiz.Model.ManyToManyRelationship;
using HaloBiz.Model.RoleManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    [Index(nameof(RoleId), Name = "IX_UserProfiles_RoleId")]
    [Index(nameof(SBUId), Name = "IX_UserProfiles_SBUId")]
    public class UserProfile
    {
        public UserProfile()
        {
            LeadEngagementUserProfiles = new HashSet<LeadEngagementUserProfile>();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(20), MinLength(2), RegularExpression("[\\w\\s\\W]{2,20}")]
        public string FirstName { get; set; }
        [RegularExpression("[\\w\\s\\W]{2,20}")]
        public string LastName { get; set; }
        [RegularExpression("[\\w\\s\\W]{2,20}")]
        public string OtherName { get; set; }
        [RegularExpression("[\\w\\s\\W]{2,20}")]
        public string CodeName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }
        [RegularExpression("\\d{10,15}")]
        public string MobileNumber { get; set; }
        [Required, MaxLength(255)]
        public string ImageUrl { get; set; }
        public string AltEmail { get; set; }
        [RegularExpression("\\d{10,15}")]
        public string AltMobileNumber { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string LinkedInHandle { get; set; }
        [MaxLength(255)]
        public string FacebookHandle  { get; set; }
        [MaxLength(255)]
        public string TwitterHandle { get; set; }
        [MaxLength(255)]
        public string InstagramHandle { get; set; }
        public long StaffId { get; set; }
        public long? SBUId { get; set; }
        public virtual StrategicBusinessUnit SBU { get; set; }
        public bool ProfileStatus { get; set; } = true;
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool SignUpMailSent { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        [InverseProperty(nameof(LeadEngagementUserProfile.UsersEngagedWith))]
        public virtual ICollection<LeadEngagementUserProfile> LeadEngagementUserProfiles { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserProfile profile &&
                   Id == profile.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            var requiredFields = $"Id = {this.Id}, \nFirstname = {this.FirstName}, \nLastname = {this.LastName}, \nEmail = {this.Email}, \n" + 
            $"MobileNumber = {this.MobileNumber}, \nImageUrl = {this.ImageUrl}, \nAddress = {this.Address},\n";
            var nonRequiredField = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", 
                String.IsNullOrEmpty(this.AltEmail) ? "" :  $"AltEmail = {this.AltEmail}, \n",
                String.IsNullOrEmpty(this.AltMobileNumber)  ? "" : $"AltMobileNumber = {this.AltMobileNumber}, \n",
                String.IsNullOrEmpty(this.LinkedInHandle)  ? "" : $"LinkedInHandle = {this.LinkedInHandle}, \n",
                String.IsNullOrEmpty(this.FacebookHandle)  ? "" : $"FacebookHandle = {this.FacebookHandle}, \n",
                String.IsNullOrEmpty(this.TwitterHandle)  ? "" : $"TwitterHandle = {this.TwitterHandle}, \n",
                String.IsNullOrEmpty(this.LinkedInHandle) ? "" : $"LinkedInHandle = {this.LinkedInHandle}, \n",
                String.IsNullOrEmpty(this.InstagramHandle) ? "" : $"InstagramHandle = {this.InstagramHandle}, \n",
                this.StaffId == 0 ? "" : $"StaffId = {this.StaffId}"
            );
            return requiredFields + nonRequiredField;
        }
    }
}