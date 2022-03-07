using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IMeansOfIdentificationRepository
    {
        Task<MeansOfIdentification> SaveMeansOfIdentification(MeansOfIdentification MeansOfIdentification);
        Task<MeansOfIdentification> FindMeansOfIdentificationById(long Id);
        Task<MeansOfIdentification> FindMeansOfIdentificationByName(string name);
        Task<IEnumerable<MeansOfIdentification>> FindAllMeansOfIdentification();
        Task<MeansOfIdentification> UpdateMeansOfIdentification(MeansOfIdentification MeansOfIdentification);
        Task<bool> DeleteMeansOfIdentification(MeansOfIdentification MeansOfIdentification);
    }
}