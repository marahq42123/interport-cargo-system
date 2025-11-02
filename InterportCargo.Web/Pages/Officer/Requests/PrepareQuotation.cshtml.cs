using System;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Officer
{
    public class PrepareQuotationModel : PageModel
    {
        private readonly InterportContext _context;
        public PrepareQuotationModel(InterportContext context) => _context = context;

        public QuotationRequest Request { get; set; } = default!;

        [BindProperty] public int RequestId { get; set; }
        [BindProperty] public string ContainerType { get; set; } = "20 Feet Container";
        [BindProperty] public string Scope { get; set; } = string.Empty;
        [BindProperty] public int NumberOfContainers { get; set; }
        [BindProperty] public bool RequiresQuarantine { get; set; }
        [BindProperty] public bool RequiresFumigation { get; set; }

        [BindProperty] public decimal ChargesBase { get; set; }
        [BindProperty] public decimal ChargesDepot { get; set; }
        [BindProperty] public decimal ChargesLcl { get; set; }
        [BindProperty] public decimal DiscountPercent { get; set; }
        [BindProperty] public decimal TotalAmount { get; set; }

        public async Task<IActionResult> OnGetAsync(int requestId)
        {
            var req = await _context.QuotationRequests
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (req == null) return NotFound();

            Request = req;
            RequestId = requestId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var req = await _context.QuotationRequests.FirstOrDefaultAsync(r => r.Id == RequestId);
            if (req == null) return NotFound();

            // Calculate charges
            (ChargesBase, ChargesDepot, ChargesLcl) = GetCharges(ContainerType);

            // Subtotal and discount
            var subtotal = ChargesBase + ChargesDepot + ChargesLcl;
            DiscountPercent = GetDiscountRate(NumberOfContainers, RequiresQuarantine, RequiresFumigation);
            var discountAmount = subtotal * (DiscountPercent / 100);
            var gst = (subtotal - discountAmount) * 0.10m;
            TotalAmount = subtotal - discountAmount + gst;

            //  if officer clicked "Calculate" button, just refresh page with calculated values
            if (action == "calculate")
            {
                // reload the same page showing totals
                Request = req;
                return Page();
            }

            //  if officer clicked "Submit", save quotation to DB
            var quotation = new Quotation
            {
                QuotationRequestId = req.Id,
                QuotationNumber = $"QTN-{DateTime.UtcNow.Ticks.ToString()[^5..]}",
                ContainerType = ContainerType,
                Scope = Scope,
                ChargesBase = ChargesBase,
                ChargesDepot = ChargesDepot,
                ChargesLcl = ChargesLcl,
                DiscountApplied = discountAmount,
                Status = QuotationStatus.Accepted,
                DateIssued = DateTime.UtcNow
            };

            _context.Quotations.Add(quotation);
            req.Status = QuotationStatus.Accepted; 
            await _context.SaveChangesAsync();

            // show confirmation directly
            ViewData["Message"] = $"Quotation submitted successfully. Total: ${TotalAmount:F2}";
            Request = req;
            return RedirectToPage("/Officer/Requests", new { id = req.Id });
        }



        // Rate schedule from the support document
        private (decimal baseCharge, decimal depotCharge, decimal lclCharge) GetCharges(string containerType)
        {
            if (containerType.Contains("40"))
            {
                // 40ft container: sum of listed base charges
                decimal baseCharge = 70 + 120 + 280 + 500 + 160 + 300 + 100 + 90; // all charges from the table
                decimal depot = 500;
                decimal lcl = 500;
                return (baseCharge, depot, lcl);
            }
            else
            {
                // 20ft container
                decimal baseCharge = 60 + 80 + 220 + 400 + 120 + 240 + 70 + 60;
                decimal depot = 400;
                decimal lcl = 400;
                return (baseCharge, depot, lcl);
            }
        }

        // Discount criteria from appendix
        private decimal GetDiscountRate(int count, bool quarantine, bool fumigation)
        {
            if (count > 10 && quarantine && fumigation) return 10m;
            if (count > 5 && quarantine && fumigation) return 5m;
            if (count > 5 && (quarantine || fumigation)) return 2.5m;
            return 0m;
        }
    }
}
