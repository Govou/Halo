﻿using Halobiz.Common.DTOs.ApiDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IInvoiceService
    {
        Task<ApiCommonResponse> GetContractInvoices(int contractServiceId);
    }
}