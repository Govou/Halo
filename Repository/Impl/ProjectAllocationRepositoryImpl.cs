using HaloBiz.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HaloBiz.Repository.Impl
{

    public class ProjectAllocationRepositoryImpl: IProjectAllocationRepositoryImpl
    {

        private readonly HalobizContext _context;
        private readonly ILogger<ProjectAllocationRepositoryImpl> _logger;
        public ProjectAllocationRepositoryImpl(HalobizContext context, ILogger<ProjectAllocationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }



        public async Task<ProjectAllocation> saveNewManager(ProjectAllocation projectAllocation)
        {
            var ProjectAllocationEntity = await _context.ProjectAllocations.AddAsync(projectAllocation);
           
            if (await SaveChanges())
            {
                return ProjectAllocationEntity.Entity;
            }
            return null;
        }


        public async Task <List<ProjectAllocation>> getAllManagerProjects(string email,int Id)
        {
            var getMyProjects = await _context.ProjectAllocations.Where(x => x.ManagerId == Id && x.ManagerEmail == email).ToListAsync();

            if (getMyProjects.Count == 0)
            {
                return null;
            }
            return getMyProjects;
        }

        public async Task<List<ProjectAllocation>> getAllProjectManager(int categoryId)
        {
            var getMyManagers = await _context.ProjectAllocations.Where(x => x.ServiceCategoryId == categoryId &&  x.IsDeleted == false).ToListAsync();

            if (getMyManagers.Count == 0)
            {
                return getMyManagers;
            }
            return getMyManagers;
        }


        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        
    }





}