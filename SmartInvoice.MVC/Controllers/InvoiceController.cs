using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInvoice.MVC.Models;
using SmartInvoice.MVC.Services;

namespace SmartInvoice.MVC.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceService _invoiceService;
        private readonly ClientService _clientService;

        public InvoiceController(InvoiceService invoiceService, ClientService clientService)
        {
            _invoiceService = invoiceService;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetInvoices();

            return View(invoices);

        }

        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetInvoiceById(id);

            if (invoice == null)
                return NotFound();

            return PartialView("_InvoiceDetailsPartial", invoice);
        }

        public async Task<IActionResult> Create()
        {
            var clients = await _clientService.GetClients();
            ViewBag.Clients = clients;
            return View(new CreateInvoiceViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceViewModel invoice)
        {
            if (!ModelState.IsValid)
            {
                var clients = await _clientService.GetClients();
                ViewBag.Clients = clients;
                return View(invoice);
            }

            var success = await _invoiceService.CreateInvoice(invoice);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to create invoice.");

                var clients = await _clientService.GetClients();
                ViewBag.Clients = clients;

                return View(invoice);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetByClient(int clientId)
        {
            var invoices = await _invoiceService.GetInvoices();

            var filtered = invoices
                .Where(i => i.Id == clientId)
                .ToList();

            return Json(filtered);
        }
    }
}