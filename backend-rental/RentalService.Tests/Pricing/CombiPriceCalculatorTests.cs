using RentalService.API.Models;
using RentalService.API.Pricing;
using Xunit;

namespace RentalService.Tests.Pricing;

public class CombiPriceCalculatorTests
{
    private readonly CombiPriceCalculator _calculator;
    private readonly PriceCalculationConfig _config;

    public CombiPriceCalculatorTests()
    {
        _calculator = new CombiPriceCalculator();
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
        int numberOfDays = 5;
        int numberOfKm = 800;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        // Formula: baseDayRental * days * 1.3 + baseKmPrice * km
        // 100 * 5 * 1.3 + 5 * 800 = 650 + 4000 = 4650
        Assert.Equal(4650m, price);
    }

    [Theory]
    [InlineData(1, 100, 630)]      // 100*1*1.3 + 5*100 = 130 + 500 = 630
    [InlineData(3, 200, 1390)]     // 100*3*1.3 + 5*200 = 390 + 1000 = 1390
    [InlineData(7, 1000, 5910)]    // 100*7*1.3 + 5*1000 = 910 + 5000 = 5910
    public void Calculate_VariousInputs_ReturnsExpectedPrice(int days, int km, decimal expectedPrice)
    {
        // Act
        var price = _calculator.Calculate(days, km, _config);

        // Assert
        Assert.Equal(expectedPrice, price);
    }

    [Fact]
    public void Calculate_OnlyDays_AppliesMultiplier()
    {
        // Arrange - 0 km
        int numberOfDays = 4;
        int numberOfKm = 0;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        // 100 * 4 * 1.3 = 520
        Assert.Equal(520m, price);
    }

    [Fact]
    public void Calculate_OnlyKm_NoMultiplier()
    {
        // Arrange - 0 days
        int numberOfDays = 0;
        int numberOfKm = 600;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        // 5 * 600 = 3000 (km part has no multiplier)
        Assert.Equal(3000m, price);
    }

    [Fact]
    public void ApplicableCategory_ReturnsCombi()
    {
        // Act
        var category = _calculator.ApplicableCategory;

        // Assert
        Assert.Equal(CarCategory.Combi, category);
    }

    [Fact]
    public void Calculate_VerifyMultiplier_Is1Point3()
    {
        // Arrange - Use simple numbers to verify multiplier
        var testConfig = new PriceCalculationConfig
        {
            BaseDayRental = 100m,
            BaseKmPrice = 0m  // Set to 0 to isolate day calculation
        };

        // Act
        var price = _calculator.Calculate(10, 0, testConfig);

        // Assert
        // 100 * 10 * 1.3 = 1300
        Assert.Equal(1300m, price);
    }
}
