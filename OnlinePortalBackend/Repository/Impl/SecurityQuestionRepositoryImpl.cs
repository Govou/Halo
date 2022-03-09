using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.Repository;
using HalobizMigrations.Data;

namespace OnlinePortalBackend.Repository.Impl
{
    public class SecurityQuestionRepositoryImpl : ISecurityQuestionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SecurityQuestionRepositoryImpl> _logger;
        public SecurityQuestionRepositoryImpl(HalobizContext context, ILogger<SecurityQuestionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<SecurityQuestion> SaveSecurityQuestion(SecurityQuestion branch)
        {
            /*var SecurityQuestionEntity = await _context.SecurityQuestions.AddAsync(branch);
            if(await SaveChanges())
            {
                return SecurityQuestionEntity.Entity;
            }*/
            return null;
        }

        public async Task<SecurityQuestion> FindSecurityQuestionById(long Id)
        {
            /*return await _context.SecurityQuestions
                .FirstOrDefaultAsync( branch => branch.Id == Id && branch.IsDeleted == false);*/
            return null;
        }

        public async Task<IEnumerable<SecurityQuestion>> FindAllSecurityQuestiones()
        {
            /*return await _context.SecurityQuestions.Where(branch => branch.IsDeleted == false)
                .ToListAsync();*/
            return null;
        }

        public async Task<SecurityQuestion> UpdateSecurityQuestion(SecurityQuestion branch)
        {
            /*var branchEntity =  _context.SecurityQuestions.Update(branch);
            if(await SaveChanges())
            {
                return branchEntity.Entity;
            }*/
            return null;
        }

        public async Task<bool> DeleteSecurityQuestion(SecurityQuestion branch)
        {           
            /*branch.IsDeleted = true;
            _context.SecurityQuestions.Update(branch);*/
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}