﻿using System;
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
            return "<tr><td class=\"item-service borderless\">{{ServiceName}}</td><td class=\"item-description borderless\">{{ServiceDescription}}</td><td class=\"item-qty borderless\">{{Quantity}}</td><td class=\"item-price borderless\">{{Amount}}</td> <td class=\"item-price borderlessleft\">{{Total}}</td></tr>";
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