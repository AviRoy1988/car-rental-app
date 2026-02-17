using RentalService.API.Models;

namespace RentalService.API.Pricing;

public class SmallCarPriceCalculator : IPriceCalculator
{
    public CarCategory ApplicableCategory => CarCategory.SmallCar;

    public decimal Calculate(int numberOfDays, int numberOfKm, PriceCalculationConfig config)
    {
        // Formula: baseDayRental * numberOfDays
        return config.BaseDayRental * numberOfDays;
    }
}
