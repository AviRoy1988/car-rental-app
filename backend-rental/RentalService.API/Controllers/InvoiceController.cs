using Microsoft.AspNetCore.Mvc;
using RentalService.API.Services;

namespace RentalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    /// <summary>
    /// Get invoice PDF for a rental by booking number
    /// </summary>
    [HttpGet("{bookingNumber}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetInvoice(string bookingNumber)
    {
        var pdfBytes = await _invoiceService.GetInvoiceByBookingNumberAsync(bookingNumber);
        return File(pdfBytes, "application/pdf", $"Invoice_{bookingNumber}.pdf");
    }
}
