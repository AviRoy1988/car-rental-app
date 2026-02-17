using System.ComponentModel.DataAnnotations;

namespace RentalService.API.Models.DTOs;

public class ReturnRentalRequest
{
    [Required(ErrorMessage = "Return date/time is required")]
    public DateTime ReturnDateTime { get; set; }

    [Required(ErrorMessage = "Return meter reading is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Meter reading must be zero or positive")]
    public int ReturnMeterReading { get; set; }
}
