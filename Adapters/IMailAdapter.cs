using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.MailDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Adapters
{
    public interface IMailAdapter
    {
        Task<ApiResponse> SendUserAssignedToRoleMail(NewRoleAssignedDTO newRoleAssignedDTO);
        Task<ApiResponse> SendNewDeliverableAssigned(NewDeliverableAssignedDTO newDeliverableAssignedDTO);
    }
}
