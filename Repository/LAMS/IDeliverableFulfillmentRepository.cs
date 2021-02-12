using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IDeliverableFulfillmentRepository
    {
        Task<DeliverableFulfillment> SaveDeliverableFulfillment(DeliverableFulfillment deliverableFulfillment);
        Task<DeliverableFulfillment> FindDeliverableFulfillmentById(long Id);
        Task<DeliverableFulfillment> FindDeliverableFulfillmentByName(string name);
        Task<IEnumerable<DeliverableFulfillment>> FindAllDeliverableFulfillment();
        Task<DeliverableFulfillment> UpdateDeliverableFulfillment(DeliverableFulfillment deliverableFulfillment);
        Task<bool> DeleteDeliverableFulfillment(DeliverableFulfillment deliverableFulfillment);
        Task<IEnumerable<DeliverableFulfillment>> FindAllDeliverableFulfillmentForTaskMaster(long taskMasterId);
    }
}