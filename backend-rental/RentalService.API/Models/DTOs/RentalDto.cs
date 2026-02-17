namespace RentalService.API.Models.DTOs;

public class RentalDto
{
    public Guid BookingNumber { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string CustomerSocialSecurityNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime PickupDateTime { get; set; }
    public int PickupMeterReading { get; set; }
    public DateTime? ReturnDateTime { get; set; }
    public int? ReturnMeterReading { get; set; }
    public decimal? CalculatedPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? NumberOfDays { get; set; }
    public int? NumberOfKm { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
}
