using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestatServer.Models.Util;

namespace TestatServer.Models
{
    public class Transaction : Persistable
    {
        public string From { get; set; }
        public string Target { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
