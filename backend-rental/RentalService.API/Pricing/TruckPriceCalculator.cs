using RentalService.API.Models;

namespace RentalService.API.Pricing;

public class TruckPriceCalculator : IPriceCalculator
{
    public CarCategory ApplicableCategory => CarCategory.Truck;

    public decimal Calculate(int numberOfDays, int numberOfKm, PriceCalculationConfig config)
    {
        // Formula: baseDayRental * numberOfDays * 1.5 + baseKmPrice * numberOfKm * 1.5
        return config.BaseDayRental * numberOfDays * 1.5m + config.BaseKmPrice * numberOfKm * 1.5m;
    }
}
