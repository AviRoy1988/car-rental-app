using RentalService.API.Models;
using RentalService.API.Pricing;
using Xunit;

namespace RentalService.Tests.Pricing;

public class SmallCarPriceCalculatorTests
{
    private readonly SmallCarPriceCalculator _calculator;
    private readonly PriceCalculationConfig _config;

    public SmallCarPriceCalculatorTests()
    {
        _calculator = new SmallCarPriceCalculator();
        _config = new PriceCalculationConfig
        {
            BaseDayRental = 100m,
            BaseKmPrice = 5m
        };
    }

    [Fact]
    public void Calculate_WithValidInput_ReturnsCorrectPrice()
    {
        // Arrange
        int numberOfDays = 3;
        int numberOfKm = 300;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        Assert.Equal(300m, price); // 100 * 3 = 300
    }

    [Theory]
    [InlineData(1, 0, 100)]     // 1 day = 100
    [InlineData(5, 0, 500)]     // 5 days = 500
    [InlineData(10, 0, 1000)]   // 10 days = 1000
    public void Calculate_VariousDays_ReturnsExpectedPrice(int days, int km, decimal expectedPrice)
    {
        // Act
        var price = _calculator.Calculate(days, km, _config);

        // Assert
        Assert.Equal(expectedPrice, price);
    }

    [Fact]
    public void Calculate_KmIsIgnored_OnlyDaysAffectPrice()
    {
        // Arrange - SmallCar doesn't use km in formula
        int numberOfDays = 4;
        int numberOfKm1 = 100;
        int numberOfKm2 = 1000;

        // Act
        var price1 = _calculator.Calculate(numberOfDays, numberOfKm1, _config);
        var price2 = _calculator.Calculate(numberOfDays, numberOfKm2, _config);

        // Assert - Both should be same (400) regardless of km
        Assert.Equal(400m, price1);
        Assert.Equal(400m, price2);
        Assert.Equal(price1, price2);
    }

    [Fact]
    public void ApplicableCategory_ReturnsSmallCar()
    {
        // Act
        var category = _calculator.ApplicableCategory;

        // Assert
        Assert.Equal(CarCategory.SmallCar, category);
    }

    [Fact]
    public void Calculate_WithZeroDays_ReturnsZero()
    {
        // Act
        var price = _calculator.Calculate(0, 100, _config);

        // Assert
        Assert.Equal(0m, price);
    }

    [Fact]
    public void Calculate_WithDifferentBaseRate_UsesNewRate()
    {
        // Arrange
        var customConfig = new PriceCalculationConfig
        {
            BaseDayRental = 150m,
            BaseKmPrice = 5m
        };

        // Act
        var price = _calculator.Calculate(2, 0, customConfig);

        // Assert
        Assert.Equal(300m, price); // 150 * 2 = 300
    }
}
