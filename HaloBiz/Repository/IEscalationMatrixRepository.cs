using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IEscalationMatrixRepository
    {
        Task<EscalationMatrix> SaveEscalationMatrix(EscalationMatrix escalationMatrix);
        Task<EscalationMatrix> FindEscalationMatrixById(long Id);
        //Task<EscalationMatrix> FindEscalationMatrixByName(string name);
        Task<IEnumerable<EscalationMatrix>> FindAllEscalationMatrixs();
        Task<EscalationMatrix> UpdateEscalationMatrix(EscalationMatrix escalationMatrix);
        Task<bool> DeleteEscalationMatrix(EscalationMatrix escalationMatrix);
    }
}
