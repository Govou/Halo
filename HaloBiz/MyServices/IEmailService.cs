using System.Collections.Generic;
using HaloBiz.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IEmailService
    {
        void Send(MailRequest mailRequest);
    }
}