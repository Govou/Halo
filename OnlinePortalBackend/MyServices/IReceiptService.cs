using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IReceiptService
    {
        public Task<bool> PostAccounts(long loggedInUserId, Receipt receipt, Invoice invoice, long bankAccountId);
    }
}
