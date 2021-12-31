using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class GroupInvoiceTrackerRepositoryImpl : IGroupInvoiceTrackerRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<GroupInvoiceTrackerRepositoryImpl> logger;


        public GroupInvoiceTrackerRepositoryImpl(HalobizContext context, ILogger<GroupInvoiceTrackerRepositoryImpl> logger)
        {
            this._context = context;
            this.logger = logger;
        }

        public async  Task<string> GenerateGroupInvoiceNumber()
        {
            try
            {
                var tracker  = await _context.GroupInvoiceTrackers.OrderBy(x => x.Id).FirstOrDefaultAsync();
                long newNumber = 0;
                if(tracker == null){
                    newNumber = 1;
                await _context.GroupInvoiceTrackers.AddAsync(new GroupInvoiceTracker(){Number = newNumber + 1});
                await  _context.SaveChangesAsync();  
                return $"GINV{newNumber.ToString().PadLeft(7, '0')}";
                }else{
                    newNumber = tracker.Number;
                    tracker.Number = tracker.Number + 1;
                    _context.GroupInvoiceTrackers.Update(tracker);
                    await _context.SaveChangesAsync();
                    return $"GINV{newNumber.ToString().PadLeft(7, '0')}";
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                return null;
            }
       
        }
        
    }
}