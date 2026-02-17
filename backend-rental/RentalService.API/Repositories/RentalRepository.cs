using Microsoft.EntityFrameworkCore;
using RentalService.API.Data;
using RentalService.API.Models;

namespace RentalService.API.Repositories;

/// <summary>
/// Rental-specific repository implementation with domain-specific queries
/// </summary>
public class RentalRepository : Repository<Rental>, IRentalRepository
{
    private readonly RentalDbContext _context;

    public RentalRepository(RentalDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Rental?> GetByBookingNumberAsync(Guid bookingNumber)
    {
        return await _context.Rentals
            .FirstOrDefaultAsync(r => r.BookingNumber == bookingNumber);
    }

    public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
    {
        return await _context.Rentals
            .Where(r => r.Status == "Active")
            .OrderByDescending(r => r.PickupDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetCompletedRentalsAsync()
    {
        return await _context.Rentals
            .Where(r => r.Status == "Completed")
            .OrderByDescending(r => r.ReturnDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByCategoryAsync(string category)
    {
        return await _context.Rentals
            .Where(r => r.CarCategory == category)
            .OrderByDescending(r => r.PickupDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Rentals
            .Where(r => r.PickupDateTime >= startDate && r.PickupDateTime <= endDate)
            .OrderBy(r => r.PickupDateTime)
            .ToListAsync();
    }

    public async Task<bool> BookingNumberExistsAsync(Guid bookingNumber)
    {
        return await _context.Rentals
            .AnyAsync(r => r.BookingNumber == bookingNumber);
    }
}
