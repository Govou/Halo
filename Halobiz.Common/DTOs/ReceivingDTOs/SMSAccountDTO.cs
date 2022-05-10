using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class SMSIndividualAccountDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int StateId { get; set; }
        public int LGAId { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public SMSLoginDTO AccountLogin { get; set; }
    }

    public class SMSLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SMSBusinessAccountDTO
    {
        public string CompanyName { get; set; }
        public int IndustryId { get; set; }
        public string Industry { get; set; }
        public string PhoneNumber { get; set; }
        public int StateId { get; set; }
        public int LGAId { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public SMSLoginDTO AccountLogin { get; set; }
        public SMSContactPerson ContactPerson { get; set; }
    }

    public class SMSContactPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
    }

}
