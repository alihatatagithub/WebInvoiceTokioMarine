using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebInvoiceTokioMarine.Models
{
    public class OrderDetail
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Supplier")]
        public string SupplierId { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Customer")]

        public string CustomerId { get; set; }

        public float Total { get; set; }
        public int Qty { get; set; } = 1;
        public Product Product { get; set; }
        public Order Order { get; set; }
        public Supplier Supplier { get; set; }
        public Customer Customer { get; set; }
    }
}