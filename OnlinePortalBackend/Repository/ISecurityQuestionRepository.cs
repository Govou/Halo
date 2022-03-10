using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Repository
{
    public interface ISecurityQuestionRepository
    {
        Task<SecurityQuestion> SaveSecurityQuestion(SecurityQuestion branch);

        Task<SecurityQuestion> FindSecurityQuestionById(long Id);       

        Task<IEnumerable<SecurityQuestion>> FindAllSecurityQuestiones();

        Task<SecurityQuestion> UpdateSecurityQuestion(SecurityQuestion branch);

        Task<bool> DeleteSecurityQuestion(SecurityQuestion branch);  
    }
}