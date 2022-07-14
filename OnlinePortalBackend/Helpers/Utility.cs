using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Globalization;
using System.Security.Cryptography;

namespace OnlinePortalBackend.Helpers
{
    public class Utility
    {
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(words);
        }


        public static string MoneyToWordsConverter(double amount)
        {
            var formatter = new CultureInfo("HA-LATN-NG");
            formatter.NumberFormat.CurrencySymbol = "₦";
            int koboPart = 0;

            //convert int to double
            var splitAmt = amount.ToString().Split('.');
            int nairaPart = int.Parse(splitAmt[0]);
            string totalInWords = NumberToWords(nairaPart) + " Naira";
            if (splitAmt.Length > 1)
            {
                koboPart = int.Parse(splitAmt[1]);
                if (koboPart > 0)
                {
                    totalInWords += " and " + NumberToWords(koboPart) + " Kobo";
                }
            }

            return totalInWords;
        }

        public static string GenerateSupplierTransRef(string idNumber)
        {
            return "SMP_" + idNumber + "_" + new Random().Next(100_000_000, 1_000_000_000).ToString();
        }

        public static string ExtractIdentificationNumber(string trxRef)
        {
            var parts = trxRef.Split('_');
            return parts[1];
        }
    }
}
