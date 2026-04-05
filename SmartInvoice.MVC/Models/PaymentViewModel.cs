namespace SmartInvoice.MVC.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
