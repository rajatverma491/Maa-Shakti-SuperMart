using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KiranaMart.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [Display(Name = "Billing Date")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Amount Paid")]
        public double AmountPaid { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public double AmountDue { get; set; }
        public double Total { get; set; }
    }
}