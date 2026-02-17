using RentalService.API.Models;

namespace RentalService.API.Repositories;

/// <summary>
/// Rental-specific repository with domain-specific queries beyond basic CRUD
/// </summary>
public interface IRentalRepository : IRepository<Rental>
{
    /// <summary>
    /// Find a rental by its unique booking number
    /// </summary>
    Task<Rental?> GetByBookingNumberAsync(Guid bookingNumber);

    /// <summary>
    /// Get all active rentals
    /// </summary>
    Task<IEnumerable<Rental>> GetActiveRentalsAsync();

    /// <summary>
    /// Get all completed rentals
    /// </summary>
    Task<IEnumerable<Rental>> GetCompletedRentalsAsync();

    /// <summary>
    /// Get rentals by car category
    /// </summary>
    Task<IEnumerable<Rental>> GetRentalsByCategoryAsync(string category);

    /// <summary>
    /// Get rentals within a date range
    /// </summary>
    Task<IEnumerable<Rental>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Check if a booking number already exists
    /// </summary>
    Task<bool> BookingNumberExistsAsync(Guid bookingNumber);
}
