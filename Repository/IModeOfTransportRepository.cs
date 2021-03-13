using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IModeOfTransportRepository
    {
        Task<ModeOfTransport> SaveModeOfTransport(ModeOfTransport modeOfTransport);
        Task<ModeOfTransport> FindModeOfTransportById(long Id);
        Task<ModeOfTransport> FindModeOfTransportByName(string name);
        Task<IEnumerable<ModeOfTransport>> FindAllModeOfTransport();
        Task<ModeOfTransport> UpdateModeOfTransport(ModeOfTransport modeOfTransport);
        Task<bool> DeleteModeOfTransport(ModeOfTransport modeOfTransport);
    }
}