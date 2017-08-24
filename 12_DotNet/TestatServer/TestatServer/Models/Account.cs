using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestatServer.Models.Util;

namespace TestatServer.Models
{
    public class Account : Persistable
    {
        public double Amount { get; set; }
        public string AccountNr { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }
    }
}
