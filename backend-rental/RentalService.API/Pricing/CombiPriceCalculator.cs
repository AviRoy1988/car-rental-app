using RentalService.API.Models;

namespace RentalService.API.Pricing;

public class CombiPriceCalculator : IPriceCalculator
{
    public CarCategory ApplicableCategory => CarCategory.Combi;

    public decimal Calculate(int numberOfDays, int numberOfKm, PriceCalculationConfig config)
    {
        // Formula: baseDayRental * numberOfDays * 1.3 + baseKmPrice * numberOfKm
        return config.BaseDayRental * numberOfDays * 1.3m + config.BaseKmPrice * numberOfKm;
    }
}
