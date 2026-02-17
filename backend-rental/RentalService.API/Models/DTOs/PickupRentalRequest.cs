using System.ComponentModel.DataAnnotations;
using RentalService.API.Models;

namespace RentalService.API.Models.DTOs;

public class PickupRentalRequest
{
    [Required(ErrorMessage = "Registration number is required")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "Registration number must be between 1 and 20 characters")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Customer social security number is required")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "SSN must be between 1 and 20 characters")]
    public string CustomerSocialSecurityNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    [EnumDataType(typeof(CarCategory), ErrorMessage = "Invalid car category")]
    public CarCategory Category { get; set; }

    [Required(ErrorMessage = "Pickup date/time is required")]
    public DateTime PickupDateTime { get; set; }

    [Required(ErrorMessage = "Pickup meter reading is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Meter reading must be zero or positive")]
    public int PickupMeterReading { get; set; }
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string EmailAddress { get; set; } = string.Empty;
}
