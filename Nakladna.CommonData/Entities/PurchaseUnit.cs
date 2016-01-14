using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public class PurchaseUnit
    {
        public string Name { get; set; }
        public double Price { get; set; }

        private Regex regex;

        public PurchaseUnit(string name, string regex, double price)
        {
            Name = name;
            Price = price;
            this.regex = new Regex(regex);
        }

        public double? ParsePrice(string value)
        {
            if (regex.IsMatch(value))
                return double.Parse(regex.Match(value).Value);

            return null;
        }
    }
}
