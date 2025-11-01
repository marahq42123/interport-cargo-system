/// <summary>
/// Handles the creation and submission of a new quotation request by a customer or guest user.
/// </summary>
public class RequestQuotationModel : PageModel
{
    private readonly InterportContext _context;

    /// <summary>
    /// Injects the database context.
    /// </summary>
    public RequestQuotationModel(InterportContext context) => _context = context;

    /// <summary>
    /// Form input model bound to the request form fields.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = new();

    /// <summary>
    /// Optional message displayed to the user after submission.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Form fields submitted by user on quotation request page.
    /// </summary>
    public class InputModel
    {
        [Required] public string Source { get; set; } = string.Empty;
        [Required] public string Destination { get; set; } = string.Empty;
        public string ContainerType { get; set; } = "20GP";
        [Range(1, 999)] public int NumberOfContainers { get; set; } = 1;
        [Required] public string PackageNature { get; set; } = "General";
        [Required] public string JobNature { get; set; } = "Import";
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Displays the quotation request form.
    /// </summary>
    public void OnGet() { }

    /// <summary>
    /// Validates and stores a new quotation request in the database.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var quotation = new QuotationRequest
        {
            CustomerId = null,
            CustomerName = "Guest User",
            Source = Input.Source,
            Destination = Input.Destination,
            ContainerType = Input.ContainerType,
            NumberOfContainers = Input.NumberOfContainers,
            PackageNature = Input.PackageNature,
            JobNature = Input.JobNature,
            Notes = Input.Notes,
            Status = QuotationStatus.Pending,
            CreatedUtc = DateTime.UtcNow
        };

        _context.QuotationRequests.Add(quotation);
        await _context.SaveChangesAsync();
        TempData["Success"] = "✅ Quotation request submitted successfully.";
        return RedirectToPage("/Customer/RequestQuotation");
    }
}
