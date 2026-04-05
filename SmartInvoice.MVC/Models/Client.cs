using System;
using System.ComponentModel.DataAnnotations;

namespace SmartInvoice.MVC.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public decimal TotalInvoiced { get; set; }

        public int TotalInvoices { get; set; }

        public int PaidInvoices { get; set; }
    }
}
