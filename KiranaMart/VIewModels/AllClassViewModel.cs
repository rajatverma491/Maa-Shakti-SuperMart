using KiranaMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KiranaMart.VIewModels
{
    public class AllClassViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public SellingHistory SellingHistory { get; set; }
        public IEnumerable<SellingHistory> SellingHistories { get; set; }
        public TodayExpenses TodayExpense { get; set; }
        public IEnumerable<TodayExpenses> TodayExpenses { get; set; }
        public Billing Billing { get; set; }
        public IEnumerable<Billing> Billings { get; set; }
        public IEnumerable<TempProduct> TempProducts { get; set; }
        public TempProduct TempProduct { get; set; }
        public double Discount { get; set; }
    }
}