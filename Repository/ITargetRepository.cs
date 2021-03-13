using HaloBiz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ITargetRepository
    {
        Task<Target> SaveTarget(Target target);
        Task<Target> FindTargetById(long Id);
        Task<Target> FindTargetByName(string name);
        Task<IEnumerable<Target>> FindAllTargets();
        Task<Target> UpdateTarget(Target target);
        Task<bool> DeleteTarget(Target target);
    }
}
