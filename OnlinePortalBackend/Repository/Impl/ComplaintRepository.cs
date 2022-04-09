using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EndorsementRepositoryImpl> _logger;

        public ComplaintRepository(HalobizContext context)
        {
            _context = context;
        }

        public Task<int> CreateComplaint()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<ComplaintTypeDTO>> GetComplainTypes()
        {
           return _context.ComplaintTypes.Select(t => new ComplaintTypeDTO
            {
                Caption = t.Caption,
                Description = t.Description,
                Id = (int)t.Id
            });
        }

        public async Task<int> GetComplaintOrigin()
        {
            var result =  _context.ComplaintOrigins.FirstOrDefault(x => x.Caption.ToLower() == "client").Id;
            return (int)result;
        }

        public async Task<int> GetComplaintSource()
        {
            var result = _context.ComplaintSources.FirstOrDefault(x => x.Caption.ToLower().Contains("web")).Id;
            return (int)result;
        }

    }
}
