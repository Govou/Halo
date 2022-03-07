using System.ComponentModel.DataAnnotations;

namespace HaloBiz.Model.AccountsModel
{
    public class GroupInvoiceTracker
    {
        [Key]
        public long Id { get; set; }        
        public long Number { get; set; }
    }
}