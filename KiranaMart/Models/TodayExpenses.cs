using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KiranaMart.Models
{
    public class TodayExpenses
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public int TodayExpence { get; set; }
        [Required]

        public String Details { get; set; }
        public DateTime Date { get; set; }
    }
}