using RentalService.API.Models;

namespace RentalService.API.Pricing;

public interface IPriceCalculator
{
    decimal Calculate(int numberOfDays, int numberOfKm, PriceCalculationConfig config);
    CarCategory ApplicableCategory { get; }
}
