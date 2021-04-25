using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ReferenceNumberRepositoryImpl : IReferenceNumberRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ReferenceNumberRepositoryImpl> _logger;

        public ReferenceNumberRepositoryImpl(HalobizContext context, ILogger<ReferenceNumberRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        public async Task<ReferenceNumber> GetReferenceNumber()
        {
            return await _context.ReferenceNumbers.FirstOrDefaultAsync(referenceNo => referenceNo.Id == 1);
        }

        public async Task<bool> UpdateReferenceNumber(ReferenceNumber refNumber)
        {
            _context.ReferenceNumbers.Update(refNumber);
            return await  _context.SaveChangesAsync() > 0;
        }
    }
}