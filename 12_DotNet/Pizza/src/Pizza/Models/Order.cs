using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pizza.Models
{
    public enum OrderState
    {
        New,
        InProgress,
        Shipped,
        Deleted
    }

    public class Order
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public OrderState State { get; set; }

        [Required]
        public virtual ApplicationUser Customer { get; set; }

        public string CustomerId { get; set; }

        public Order()
        {
            Date = DateTime.Now;
        }
    }
}
