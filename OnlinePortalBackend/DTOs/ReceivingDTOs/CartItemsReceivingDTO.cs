using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class CartItemsReceivingDTO
    {
        public List<CartItemReceivingDTO> CartItems { get; set; }
    }

    public class CartItemReceivingDTO
    {
        public ProductReceivingDTO Product { get; set; }
        public IDictionary<string, object> FormData { get; set; }
        public long ProspectId { get; set; }
        public long Quantity { get; set; }
        public double Total { get; set; }
        public long Duration { get; set; }
        public double Vat { get; set; }
        public double Amount { get; set; }
    }

    public class ProductReceivingDTO
    {
        public long ServiceId { get; set; }
    }

    /*public class FormDataReceivingDTO
    {
        public long Quantity { get; set; }
        public long Duration { get; set; }
    }*/
}
