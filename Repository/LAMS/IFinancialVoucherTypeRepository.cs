using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IFinancialVoucherTypeRepository
    {
         Task<FinanceVoucherType> SaveFinanceVoucherType(FinanceVoucherType voucherType);
         Task<FinanceVoucherType> FindFinanceVoucherTypeById(long Id);
         Task<IEnumerable<FinanceVoucherType>> FindAllFinanceVoucherType();
         Task<FinanceVoucherType> UpdateFinanceVoucherType(FinanceVoucherType voucherType);
         Task<bool> DeleteFinanceVoucherType(FinanceVoucherType voucherType);
    }
}