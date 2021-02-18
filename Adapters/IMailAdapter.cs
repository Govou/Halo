using HaloBiz.DTOs.ApiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Adapters
{
    public interface IMailAdapter
    {
        Task<ApiResponse> SendUserAssignedToRoleMail(string userEmail, string userName);
    }
}
