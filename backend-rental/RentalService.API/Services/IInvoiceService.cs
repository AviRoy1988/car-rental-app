namespace RentalService.API.Services;

public interface IInvoiceService
{
    Task<byte[]> GetInvoiceByBookingNumberAsync(string bookingNumber);
}