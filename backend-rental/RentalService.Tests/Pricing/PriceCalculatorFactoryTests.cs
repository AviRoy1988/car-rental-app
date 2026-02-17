using RentalService.API.Models;
using RentalService.API.Pricing;
using Xunit;

namespace RentalService.Tests.Pricing;

public class PriceCalculatorFactoryTests
{
    [Fact]
    public void GetCalculator_WithSmallCar_ReturnsSmallCarCalculator()
    {
        // Arrange
        var calculators = new List<IPriceCalculator>
        {
            new SmallCarPriceCalculator(),
            new CombiPriceCalculator(),
            new TruckPriceCalculator()
        };
        var factory = new PriceCalculatorFactory(calculators);

        // Act
        var calculator = factory.GetCalculator(CarCategory.SmallCar);

        // Assert
        Assert.NotNull(calculator);
        Assert.IsType<SmallCarPriceCalculator>(calculator);
        Assert.Equal(CarCategory.SmallCar, calculator.ApplicableCategory);
    }

    [Fact]
    public void GetCalculator_WithCombi_ReturnsCombiCalculator()
    {
        // Arrange
        var calculators = new List<IPriceCalculator>
        {
            new SmallCarPriceCalculator(),
            new CombiPriceCalculator(),
            new TruckPriceCalculator()
        };
        var factory = new PriceCalculatorFactory(calculators);

        // Act
        var calculator = factory.GetCalculator(CarCategory.Combi);

        // Assert
        Assert.NotNull(calculator);
        Assert.IsType<CombiPriceCalculator>(calculator);
        Assert.Equal(CarCategory.Combi, calculator.ApplicableCategory);
    }

    [Fact]
    public void GetCalculator_WithTruck_ReturnsTruckCalculator()
    {
        // Arrange
        var calculators = new List<IPriceCalculator>
        {
            new SmallCarPriceCalculator(),
            new CombiPriceCalculator(),
            new TruckPriceCalculator()
        };
        var factory = new PriceCalculatorFactory(calculators);

        // Act
        var calculator = factory.GetCalculator(CarCategory.Truck);

        // Assert
        Assert.NotNull(calculator);
        Assert.IsType<TruckPriceCalculator>(calculator);
        Assert.Equal(CarCategory.Truck, calculator.ApplicableCategory);
    }

    [Fact]
    public void GetCalculator_WithInvalidCategory_ThrowsException()
    {
        // Arrange
        var calculators = new List<IPriceCalculator>
        {
            new SmallCarPriceCalculator(),
            new CombiPriceCalculator()
            // Note: TruckPriceCalculator intentionally missing
        };
        var factory = new PriceCalculatorFactory(calculators);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            factory.GetCalculator(CarCategory.Truck));

        Assert.Contains("No price calculator found for category: Truck", exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyList_CreatesFactory()
    {
        // Arrange & Act
        var calculators = new List<IPriceCalculator>();
        var factory = new PriceCalculatorFactory(calculators);

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public void GetCalculator_MultipleCalls_ReturnsSameCalculatorInstance()
    {
        // Arrange
        var calculators = new List<IPriceCalculator>
        {
            new SmallCarPriceCalculator(),
            new CombiPriceCalculator(),
            new TruckPriceCalculator()
        };
        var factory = new PriceCalculatorFactory(calculators);

        // Act
        var calc1 = factory.GetCalculator(CarCategory.SmallCar);
        var calc2 = factory.GetCalculator(CarCategory.SmallCar);

        // Assert - Should return same instance from dictionary
        Assert.Same(calc1, calc2);
    }
}
