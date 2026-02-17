namespace RentalService.API.Models;

public class Rental
{
    public int Id { get; set; }
    public Guid BookingNumber { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string CustomerSocialSecurityNumber { get; set; } = string.Empty;
    public CarCategory Category { get; set; }
    public DateTime PickupDateTime { get; set; }
    public int PickupMeterReading { get; set; }
    public DateTime? ReturnDateTime { get; set; }
    public int? ReturnMeterReading { get; set; }
    public decimal? CalculatedPrice { get; set; }
    public RentalStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
}
