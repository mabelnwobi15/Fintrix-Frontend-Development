using Microsoft.AspNetCore.Mvc;
using SmartInvoice.MVC.Models;
using SmartInvoice.MVC.Services;
using System.Diagnostics;

namespace SmartInvoice.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly InvoiceService _invoiceService;
        private readonly PaymentService _paymentService;
        private readonly ClientService _clientService;

        public HomeController(InvoiceService invoiceService,PaymentService paymentService,ClientService clientService)
        {
            _invoiceService = invoiceService;
            _paymentService = paymentService;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetInvoices();
            var payments = await _paymentService.GetPaymentsByUser();
            var clients = await _clientService.GetClients();

            ViewBag.TotalRevenue = payments.Sum(p => p.Amount);
            ViewBag.TotalInvoices = invoices.Count;
            ViewBag.TotalClients = clients.Count;

            ViewBag.PendingInvoices = invoices.Count(i => i.Status == "Pending");
            ViewBag.PaidInvoices = invoices.Count(i => i.Status == "Paid");
            ViewBag.PartialInvoices = invoices.Count(i => i.Status == "Overdue");

            var totalInvoicesAmount = invoices.Sum(i => i.TotalAmount);
            var totalPaid = payments.Sum(p => p.Amount);

            ViewBag.RemainingBalance = totalInvoicesAmount - totalPaid;

            ViewBag.Overdue = invoices.Count(i =>
                i.DueDate < DateTime.Now && i.Status != "Paid");

            ViewBag.RecentPayments = payments.OrderByDescending(p => p.PaymentDate).Take(5).ToList();

            ViewBag.RecentPayments = payments.OrderByDescending(p => p.PaymentDate)
            .Take(5)
            .ToList();

            ViewBag.RecentPayments = payments.OrderByDescending(p => p.PaymentDate)
            .Take(5)
            .ToList();

            var grouped = payments.GroupBy(p => p.PaymentDate.ToString("MMM")).Select(g => new
            {
                Month = g.Key,
                Total = g.Sum(x => x.Amount),
                Count = g.Count()
            }).ToList();

            ViewBag.RevenueData = new
            {
                labels = grouped.Select(g => g.Month),
                values = grouped.Select(g => g.Total)
            };

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


