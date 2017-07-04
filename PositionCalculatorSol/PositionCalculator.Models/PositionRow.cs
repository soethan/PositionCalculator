using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionCalculator.Models
{
    public class PositionRow
    {
        public string Trader { get; set; }
        public string Broker { get; set; }
        public string Symbol { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
