using RentalService.API.Models;

namespace RentalService.API.Pricing;

public class PriceCalculatorFactory
{
    private readonly Dictionary<CarCategory, IPriceCalculator> _calculators;

    public PriceCalculatorFactory(IEnumerable<IPriceCalculator> calculators)
    {
        _calculators = calculators.ToDictionary(c => c.ApplicableCategory);
    }

    public IPriceCalculator GetCalculator(CarCategory category)
    {
        if (_calculators.TryGetValue(category, out var calculator))
            return calculator;

        throw new InvalidOperationException($"No price calculator found for category: {category}");
    }
}
