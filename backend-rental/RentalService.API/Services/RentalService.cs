using AutoMapper;
using Microsoft.Extensions.Options;
using RentalService.API.Models;
using RentalService.API.Models.DTOs;
using RentalService.API.Pricing;
using RentalService.API.Repositories;

namespace RentalService.API.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _repository;
    private readonly PriceCalculatorFactory _priceCalculatorFactory;
    private readonly PriceCalculationConfig _priceConfig;
    private readonly IMapper _mapper;

    public RentalService(
        IRentalRepository repository,
        PriceCalculatorFactory priceCalculatorFactory,
        IOptions<PriceCalculationConfig> priceConfig,
        IMapper mapper)
    {
        _repository = repository;
        _priceCalculatorFactory = priceCalculatorFactory;
        _priceConfig = priceConfig.Value;
        _mapper = mapper;
    }

    public async Task<RentalDto> RegisterPickupAsync(PickupRentalRequest request)
    {
        var rental = new Rental
        {
            BookingNumber = Guid.NewGuid(),
            RegistrationNumber = request.RegistrationNumber,
            CustomerSocialSecurityNumber = request.CustomerSocialSecurityNumber,
            Category = request.Category,
            PickupDateTime = request.PickupDateTime,
            PickupMeterReading = request.PickupMeterReading,
            Status = RentalStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(rental);

        return _mapper.Map<RentalDto>(rental);
    }

    public async Task<RentalDto> RegisterReturnAsync(string bookingNumber, ReturnRentalRequest request)
    {
        if (!Guid.TryParse(bookingNumber, out var bookingGuid))
        {
            throw new InvalidOperationException($"Invalid booking number format: '{bookingNumber}'");
        }

        var rentals = await _repository.FindAsync(r => r.BookingNumber == bookingGuid);
        var rental = rentals.FirstOrDefault();

        if (rental == null)
        {
            throw new InvalidOperationException($"Rental with booking number '{bookingNumber}' not found.");
        }

        if (rental.Status != RentalStatus.Active)
        {
            throw new InvalidOperationException($"Rental with booking number '{bookingNumber}' is already completed.");
        }

        // Validation
        if (request.ReturnDateTime < rental.PickupDateTime)
        {
            throw new InvalidOperationException("Return date cannot be before pickup date.");
        }

        if (request.ReturnMeterReading < rental.PickupMeterReading)
        {
            throw new InvalidOperationException("Return meter reading cannot be less than pickup meter reading.");
        }

        // Update rental with return information
        rental.ReturnDateTime = request.ReturnDateTime;
        rental.ReturnMeterReading = request.ReturnMeterReading;
        rental.UpdatedAt = DateTime.UtcNow;

        // Calculate price
        rental.CalculatedPrice = CalculatePrice(rental);
        rental.Status = RentalStatus.Completed;

        await _repository.UpdateAsync(rental);

        return _mapper.Map<RentalDto>(rental);
    }

    public async Task<RentalDto?> GetRentalByBookingNumberAsync(string bookingNumber)
    {
        if (!Guid.TryParse(bookingNumber, out var bookingGuid))
        {
            return null;
        }

        var rentals = await _repository.FindAsync(r => r.BookingNumber == bookingGuid);
        var rental = rentals.FirstOrDefault();

        return rental == null ? null : _mapper.Map<RentalDto>(rental);
    }

    public async Task<IEnumerable<RentalDto>> GetAllRentalsAsync()
    {
        var rentals = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RentalDto>>(rentals);
    }

    public async Task<IEnumerable<RentalDto>> GetActiveRentalsAsync()
    {
        var rentals = await _repository.FindAsync(r => r.Status == RentalStatus.Active);
        return _mapper.Map<IEnumerable<RentalDto>>(rentals);
    }

    private decimal CalculatePrice(Rental rental)
    {
        var numberOfDays = (int)Math.Ceiling((rental.ReturnDateTime!.Value - rental.PickupDateTime).TotalDays);
        var numberOfKm = rental.ReturnMeterReading!.Value - rental.PickupMeterReading;

        var calculator = _priceCalculatorFactory.GetCalculator(rental.Category);
        return calculator.Calculate(numberOfDays, numberOfKm, _priceConfig);
    }
}
