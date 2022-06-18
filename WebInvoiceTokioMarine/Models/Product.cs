using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebInvoiceTokioMarine.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }

        public Supplier Supplier { get; set; }
        [ForeignKey("Supplier")]
        public string SupplierId { get; set; }

        public Category Category { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [NotMapped]
        public HttpPostedFileBase Upload { get; set; }

    }
}