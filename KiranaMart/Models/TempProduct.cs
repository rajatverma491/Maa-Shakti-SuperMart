using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KiranaMart.Models
{
    public class TempProduct
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        [Required]
        public string BarCodeId { get; set; }


        public string Unit { get; set; }

        public int Price { get; set; }

        public DateTime ExpiryDate { get; set; }

        public double Quantity { get; set; }

        public string Brand { get; set; }
        public double QtyPurchased { get; set; }
    }
}