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


        ProjectAllocation projectAllocation = new ProjectAllocation();

       


    }





}