using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Files
{
    public class CsvStockQuote
    {
        public string Symbol { get; set; }

        public decimal Close { get; set; }
    }
}
