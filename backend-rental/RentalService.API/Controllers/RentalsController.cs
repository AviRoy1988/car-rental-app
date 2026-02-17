using Microsoft.AspNetCore.Mvc;
using RentalService.API.Models.DTOs;
using RentalService.API.Services;

namespace RentalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    /// <summary>
    /// Register a car pickup
    /// </summary>
    [HttpPost("pickup")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentalDto>> RegisterPickup([FromBody] PickupRentalRequest request)
    {
        var rental = await _rentalService.RegisterPickupAsync(request);
        return CreatedAtAction(nameof(GetRentalByBookingNumber), new { bookingNumber = rental.BookingNumber }, rental);
    }

    /// <summary>
    /// Register a car return
    /// </summary>
    [HttpPost("{bookingNumber}/return")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentalDto>> RegisterReturn(string bookingNumber, [FromBody] ReturnRentalRequest request)
    {
        var rental = await _rentalService.RegisterReturnAsync(bookingNumber, request);
        return Ok(rental);
    }

    /// <summary>
    /// Get rental by booking number
    /// </summary>
    [HttpGet("{bookingNumber}")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalDto>> GetRentalByBookingNumber(string bookingNumber)
    {
        var rental = await _rentalService.GetRentalByBookingNumberAsync(bookingNumber);
        if (rental == null)
        {
            return NotFound(new { error = "Rental not found" });
        }
        return Ok(rental);
    }

    /// <summary>
    /// Get all rentals
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RentalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetAllRentals()
    {
        var rentals = await _rentalService.GetAllRentalsAsync();
        return Ok(rentals);
    }

    /// <summary>
    /// Get active rentals only
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<RentalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetActiveRentals()
    {
        var rentals = await _rentalService.GetActiveRentalsAsync();
        return Ok(rentals);
    }
}
