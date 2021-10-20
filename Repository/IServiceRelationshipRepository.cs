using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
   

    public interface IServiceRelationshipRepository
    {
        Task<ServiceRelationship> SaveService(ServiceRelationship service);
        Task<ServiceRelationship> FindServiceRelationshipByAdminId(long Id);
        Task<ServiceRelationship> FindServiceRelationshipByDirectId(long Id);
        Task<IEnumerable<Service>> FindAllUnmappedDirects();
        Task<IEnumerable<ServiceRelationship>> FindAllRelationships();
    }
}
