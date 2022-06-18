using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebInvoiceTokioMarine.Models.ViewModels
{
    public class AddToCartGetViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name ="Picture")]
        public string PictureUrl { get; set; }

        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }
        public string ProductDescription { get; set; }

        public string SupplierId { get; set; }

        public float Price { get; set; }
        public string CustomerId { get; set; }
        [Required(ErrorMessage = "*")]
        [Range(1, 600)]
        [Display(Name = "Quantity")]

        public int Qty { get; set; } = 1;
        public float Total { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

    }
}