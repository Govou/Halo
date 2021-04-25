using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HaloBiz.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository
{
    public interface IDivisionRepository
    {
        Task<Division> SaveDivision(Division division);

        Task<Division> FindDivisionById(long Id);

        Task<Division> FindDivisionByName(string name);

        Task<IEnumerable<Division>> FindAllDivisions();

        Task<Division> UpdateDivision(Division division);

        Task<bool> RemoveDivision(Division division);
        Task<IEnumerable<Division>> GetAllDivisionAndSbu();

    }
}