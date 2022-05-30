using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.Helpers
{
    public class HTMLGenerator
    {
        public static string GenerateReceiptDetail()
        {
            return "<tr><td class=\"item-service \">{{ServiceName}}</td><td class=\"item-description \">{{ServiceDescription}}</td><td class=\"item-qty \">{{Quantity}}</td><td class=\"item-price \">{{Amount}}</td> <td class=\"item-price left\">{{Total}}</td></tr>";
        }

        public static string GenerateReceiptSummary()
        {
            return "<tr><td colspan = \"2\" class=\"centerText boldText\"><b>Amount:</b></td><td colspan = \"3\" class=\"centerText boldText\"><b><span>&#8358;</span> {{Amount}}</b></td></tr> <tr><td colspan = \"2\" class=\"centerText boldText\"><b>Date:</b></td> <td colspan = \"3\" class=\"centerText boldText\"><b>{{date}}</b></td></tr>";
        }

        public static string GenerateReceiptDetailsFooter()
        {
            return "<td colspan=\"5\"><b>{{AmountInWords}}</td>";
        }
    }
}
