using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Data;

namespace OnlinePortalBackend.Repository.Impl
{
    public class UserFriendlyQuestionRepositoryImpl : IUserFriendlyQuestionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<UserFriendlyQuestionRepositoryImpl> _logger;
        public UserFriendlyQuestionRepositoryImpl(HalobizContext context, ILogger<UserFriendlyQuestionRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<UserFriendlyQuestion> SaveUserFriendlyQuestion(UserFriendlyQuestion userFriendlyQuestion)
        {
            /*var userFriendlyQuestionEntity = await _context.UserFriendlyQuestions.AddAsync(userFriendlyQuestion);
            if(await SaveChanges())
            {
                return userFriendlyQuestionEntity.Entity;
            }*/
            return null;
        }

        public async Task<bool> UpdateUserFriendlyQuestions(IEnumerable<UserFriendlyQuestion> userFriendlyQuestions)
        {
            //_context.UserFriendlyQuestions.UpdateRange(userFriendlyQuestions);
            return await SaveChanges();
        }

        public async Task<UserFriendlyQuestion> FindUserFriendlyQuestionById(long Id)
        {
            /*return await _context.UserFriendlyQuestions
                .FirstOrDefaultAsync( user => user.Id == Id && user.IsDeleted == false);*/
            return null;
        }

        public async Task<IEnumerable<UserFriendlyQuestion>> FindAllUserFriendlyQuestions()
        {
            /*return await _context.UserFriendlyQuestions
                .Where(user => user.IsDeleted == false)
                .ToListAsync();*/
            return null;
        }

        public async Task<UserFriendlyQuestion> UpdateUserFriendlyQuestion(UserFriendlyQuestion userFriendlyQuestion)
        {
            /*var UserFriendlyQuestionEntity =  _context.UserFriendlyQuestions.Update(userFriendlyQuestion);
            if(await SaveChanges())
            {
                return UserFriendlyQuestionEntity.Entity;
            }*/
            return null;
        }

        public async Task<bool> RemoveUserFriendlyQuestion(UserFriendlyQuestion userFriendlyQuestion)
        {
            /*userFriendlyQuestion.IsDeleted = true;
            _context.UserFriendlyQuestions.Update(userFriendlyQuestion);*/
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}