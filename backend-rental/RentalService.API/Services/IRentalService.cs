using RentalService.API.Models.DTOs;

namespace RentalService.API.Services;

public interface IRentalService
{
    Task<RentalDto> RegisterPickupAsync(PickupRentalRequest request);
    Task<RentalDto> RegisterReturnAsync(string bookingNumber, ReturnRentalRequest request);
    Task<RentalDto?> GetRentalByBookingNumberAsync(string bookingNumber);
    Task<IEnumerable<RentalDto>> GetAllRentalsAsync();
    Task<IEnumerable<RentalDto>> GetActiveRentalsAsync();
}
