using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebInvoiceTokioMarine.Models
{
    public class Supplier : ApplicationUser
    {
        public ICollection<Product> Products { get; set; }
    }
}