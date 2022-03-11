using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class CodeVerifyModel
    {
        //string email, string code
        public string Email { get; set; }
        public string Code { get; set; }
        public CodePurpose Purpose { get; set; }
    }
}
