using System;
using HaloBiz.Models;

namespace Halobiz.Common.MyServices.RoleManagement
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}