using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KiranaMart.Models
{
    public class SellingHistory
    {
        public int Id { get; set; }
        public Billing Billing { get; set; }
        public int BillingId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public int Margin { get; set; }
        public int SellingPrice { get; set; }
        public int CostPrice { get; set; }
        public int MRP { get; set; }
    }
}