using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebInvoiceTokioMarine.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Customer")]
        public string CreatedBy { get; set; }
        public Customer Customer { get; set; }
        public float TotalOrder { get; set; } = 0;

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}