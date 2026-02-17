using RentalService.API.Models;
using RentalService.API.Pricing;
using Xunit;

namespace RentalService.Tests.Pricing;

public class TruckPriceCalculatorTests
{
    private readonly TruckPriceCalculator _calculator;
    private readonly PriceCalculationConfig _config;

    public TruckPriceCalculatorTests()
    {
        _calculator = new TruckPriceCalculator();
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
        int numberOfDays = 7;
        int numberOfKm = 1200;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        // Formula: baseDayRental * days * 1.5 + baseKmPrice * km * 1.5
        // 100 * 7 * 1.5 + 5 * 1200 * 1.5 = 1050 + 9000 = 10050
        Assert.Equal(10050m, price);
    }

    [Theory]
    [InlineData(1, 100, 900)]      // 100*1*1.5 + 5*100*1.5 = 150 + 750 = 900
    [InlineData(3, 500, 4200)]     // 100*3*1.5 + 5*500*1.5 = 450 + 3750 = 4200
    [InlineData(5, 800, 6750)]     // 100*5*1.5 + 5*800*1.5 = 750 + 6000 = 6750
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
        // Arrange
        int numberOfDays = 4;
        int numberOfKm = 0;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        // 100 * 4 * 1.5 = 600
        Assert.Equal(600m, price);
    }

    [Fact]
    public void Calculate_OnlyKm_AppliesMultiplier()
    {
        // Arrange
        int numberOfDays = 0;
        int numberOfKm = 800;

        // Act
        var price = _calculator.Calculate(numberOfDays, numberOfKm, _config);

        // Assert
        // 5 * 800 * 1.5 = 6000
        Assert.Equal(6000m, price);
    }

    [Fact]
    public void ApplicableCategory_ReturnsTruck()
    {
        // Act
        var category = _calculator.ApplicableCategory;

        // Assert
        Assert.Equal(CarCategory.Truck, category);
    }

    [Fact]
    public void Calculate_VerifyBothMultipliers_Are1Point5()
    {
        // Arrange - Use round numbers to verify multipliers
        var testConfig = new PriceCalculationConfig
        {
            BaseDayRental = 100m,
            BaseKmPrice = 10m
        };

        // Act
        var priceWithDays = _calculator.Calculate(2, 0, testConfig);
        var priceWithKm = _calculator.Calculate(0, 100, testConfig);

        // Assert
        Assert.Equal(300m, priceWithDays);   // 100 * 2 * 1.5 = 300
        Assert.Equal(1500m, priceWithKm);    // 10 * 100 * 1.5 = 1500
    }

    [Fact]
    public void Calculate_TruckIsMostExpensive_ComparedToOtherCategories()
    {
        // Arrange
        var smallCarCalc = new SmallCarPriceCalculator();
        var combiCalc = new CombiPriceCalculator();
        int days = 5;
        int km = 500;

        // Act
        var smallCarPrice = smallCarCalc.Calculate(days, km, _config);
        var combiPrice = combiCalc.Calculate(days, km, _config);
        var truckPrice = _calculator.Calculate(days, km, _config);

        // Assert - Truck should be most expensive
        Assert.True(truckPrice > smallCarPrice);
        Assert.True(truckPrice > combiPrice);
    }
}
