using Halobiz.Common.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class SbutoContractServiceProportionTransferDTO
    {
        public long Id { get; set; }
        public double Proportion { get; set; }
        public ProportionStatusType Status { get; set; }
        public SBUWithoutOperatingEntityTransferDTO StrategicBusinessUnit { get; set; }
        public UserProfileTransferDTO UserInvolved { get; set; }
        public long ContractServiceId { get; set; }
    }
    public class SBUWithoutOperatingEntityTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public enum ProportionStatusType
    {
        LeadGenerator, LeadClosure, LeadGeneratorAndClosure
    }

    //public class UserProfileTransferDTO
    //{
    //    public long Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string OtherName { get; set; }
    //    public string CodeName { get; set; }
    //    public string DateOfBirth { get; set; }
    //    public string Email { get; set; }
    //    public string MobileNumber { get; set; }
    //    public string ImageUrl { get; set; }
    //    public string AltEmail { get; set; }
    //    public string AltMobileNumber { get; set; }
    //    public string Address { get; set; }
    //    public string LinkedInHandle { get; set; }
    //    public string FacebookHandle { get; set; }
    //    public string TwitterHandle { get; set; }
    //    public string InstagramHandle { get; set; }
    //    public long StaffId { get; set; }
    //    // public StrategicBusinessUnitTransferDTO SBU { get; set; }
    //    public long? SBUId { get; set; }
    //    public bool ProfileStatus { get; set; }
    //    public bool HasSetPassword { get; set; }


    //    public override string ToString()
    //    {
    //        var requiredFields = $"Id = {this.Id}, \nFirstname = {this.FirstName}, \nLastname = {this.LastName}, \nEmail = {this.Email}, \n" +
    //         $"MobileNumber = {this.MobileNumber}, \nImageUrl = {this.ImageUrl}, \nAddress = {this.Address},\n";
    //        var nonRequiredField = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
    //            String.IsNullOrEmpty(this.AltEmail) ? "" : $"AltEmail = {this.AltEmail}, \n",
    //            String.IsNullOrEmpty(this.AltMobileNumber) ? "" : $"AltMobileNumber = {this.AltMobileNumber}, \n",
    //            String.IsNullOrEmpty(this.LinkedInHandle) ? "" : $"LinkedInHandle = {this.LinkedInHandle}, \n",
    //            String.IsNullOrEmpty(this.FacebookHandle) ? "" : $"FacebookHandle = {this.FacebookHandle}, \n",
    //            String.IsNullOrEmpty(this.TwitterHandle) ? "" : $"TwitterHandle = {this.TwitterHandle}, \n",
    //            String.IsNullOrEmpty(this.LinkedInHandle) ? "" : $"LinkedInHandle = {this.LinkedInHandle}, \n",
    //            String.IsNullOrEmpty(this.InstagramHandle) ? "" : $"InstagramHandle = {this.InstagramHandle}, \n",
    //            this.StaffId == 0 ? "" : $"StaffId = {this.StaffId}"
    //        );
    //        return requiredFields + nonRequiredField;
    //    }
    //}
}
