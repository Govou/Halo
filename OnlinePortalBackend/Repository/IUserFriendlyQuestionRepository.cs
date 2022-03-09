using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Repository
{
    public interface IUserFriendlyQuestionRepository
    {
        Task<UserFriendlyQuestion> SaveUserFriendlyQuestion(UserFriendlyQuestion userFriendlyQuestion);
        Task<bool> UpdateUserFriendlyQuestions(IEnumerable<UserFriendlyQuestion> userFriendlyQuestion);
        Task<UserFriendlyQuestion> FindUserFriendlyQuestionById(long Id);
        Task<IEnumerable<UserFriendlyQuestion>> FindAllUserFriendlyQuestions();
        Task<UserFriendlyQuestion> UpdateUserFriendlyQuestion(UserFriendlyQuestion userFriendlyQuestion);
        Task<bool> RemoveUserFriendlyQuestion(UserFriendlyQuestion userFriendlyQuestion);
    }
}