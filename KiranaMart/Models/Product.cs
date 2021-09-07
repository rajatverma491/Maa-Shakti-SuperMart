using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KiranaMart.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Bar Code")]
        public string BarCodeId { get; set; }
        [Required]

        public string Unit { get; set; }

        [Required]
        public string Brand { get; set; }
        [Display(Name = "Selling Price")]
        public int Price { get; set; }
        [Display(Name = "Cost Price")]
        public int CostPrice { get; set; }
        [Required]
        public int MRP { get; set; }

        public DateTime ExpiryDate { get; set; }
        [Required]
        public double Quantity { get; set; }
    }
}