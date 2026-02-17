using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RentalService.API.Models.DTOs;

namespace RentalService.API.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IRentalService _rentalService;

    public InvoiceService(IRentalService rentalService)
    {
        _rentalService = rentalService;
        // Configure QuestPDF license (Community license for non-commercial use)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GetInvoiceByBookingNumberAsync(string bookingNumber)
    {
        var rental = await _rentalService.GetRentalByBookingNumberAsync(bookingNumber);
        
        if (rental == null)
        {
            throw new InvalidOperationException($"Rental with booking number {bookingNumber} not found");
        }

        // Generate PDF and return as byte array
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, rental));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Text("CAR RENTAL INVOICE")
                .FontSize(24).Bold().FontColor(Colors.Blue.Darken2);
            
            column.Item().PaddingTop(10).AlignCenter().Text("Premium Car Rental Services")
                .FontSize(12).FontColor(Colors.Grey.Darken1);
                
            column.Item().PaddingTop(5).LineHorizontal(2).LineColor(Colors.Blue.Darken2);
        });
    }

    private void ComposeContent(IContainer container, RentalDto rental)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Invoice details section
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(leftColumn =>
                {
                    leftColumn.Item().Text("Invoice Details").Bold().FontSize(14);
                    leftColumn.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span("Booking Number: ").Bold();
                        text.Span(rental.BookingNumber.ToString());
                    });
                    leftColumn.Item().Text(text =>
                    {
                        text.Span("Invoice Date: ").Bold();
                        text.Span(DateTime.Now.ToString("MMMM dd, yyyy"));
                    });
                    leftColumn.Item().Text(text =>
                    {
                        text.Span("Status: ").Bold();
                        text.Span(rental.Status);
                    });
                });

                row.RelativeItem().Column(rightColumn =>
                {
                    rightColumn.Item().Text("Customer Information").Bold().FontSize(14);
                    rightColumn.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span("SSN: ").Bold();
                        text.Span(rental.CustomerSocialSecurityNumber);
                    });
                });
            });

            // Rental details section
            column.Item().PaddingTop(20).Text("Rental Details").Bold().FontSize(14);
            column.Item().PaddingTop(10).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(3);
                });

                table.Cell().Border(1).Padding(8).Background(Colors.Grey.Lighten3).Text("Field").Bold();
                table.Cell().Border(1).Padding(8).Background(Colors.Grey.Lighten3).Text("Value").Bold();

                table.Cell().Border(1).Padding(8).Text("Registration Number");
                table.Cell().Border(1).Padding(8).Text(rental.RegistrationNumber);

                table.Cell().Border(1).Padding(8).Text("Category");
                table.Cell().Border(1).Padding(8).Text(rental.Category);

                table.Cell().Border(1).Padding(8).Text("Pickup Date/Time");
                table.Cell().Border(1).Padding(8).Text(rental.PickupDateTime.ToString("yyyy-MM-dd HH:mm"));

                table.Cell().Border(1).Padding(8).Text("Pickup Meter Reading");
                table.Cell().Border(1).Padding(8).Text($"{rental.PickupMeterReading:N0} km");

                if (rental.ReturnDateTime.HasValue)
                {
                    table.Cell().Border(1).Padding(8).Text("Return Date/Time");
                    table.Cell().Border(1).Padding(8).Text(rental.ReturnDateTime.Value.ToString("yyyy-MM-dd HH:mm"));
                }

                if (rental.ReturnMeterReading.HasValue)
                {
                    table.Cell().Border(1).Padding(8).Text("Return Meter Reading");
                    table.Cell().Border(1).Padding(8).Text($"{rental.ReturnMeterReading.Value:N0} km");
                }

                if (rental.NumberOfDays.HasValue)
                {
                    table.Cell().Border(1).Padding(8).Text("Number of Days");
                    table.Cell().Border(1).Padding(8).Text(rental.NumberOfDays.Value.ToString());
                }

                if (rental.NumberOfKm.HasValue)
                {
                    table.Cell().Border(1).Padding(8).Text("Distance Traveled");
                    table.Cell().Border(1).Padding(8).Text($"{rental.NumberOfKm.Value:N0} km");
                }
            });

            // Price section
            if (rental.CalculatedPrice.HasValue)
            {
                column.Item().PaddingTop(20).AlignRight().Column(priceColumn =>
                {
                    priceColumn.Item().Border(2).BorderColor(Colors.Blue.Darken2)
                        .Background(Colors.Blue.Lighten4).Padding(15)
                        .AlignCenter().Text(text =>
                        {
                            text.Span("Total Amount: ").Bold().FontSize(16);
                            text.Span($"{rental.CalculatedPrice.Value:C2}").Bold().FontSize(18)
                                .FontColor(Colors.Blue.Darken3);
                        });
                });
            }

            // Terms and conditions
            column.Item().PaddingTop(30).Text("Terms & Conditions").Bold().FontSize(12);
            column.Item().PaddingTop(5).Text("• Payment is due upon return of the vehicle.")
                .FontSize(9).FontColor(Colors.Grey.Darken1);
            column.Item().Text("• Late returns may incur additional charges.")
                .FontSize(9).FontColor(Colors.Grey.Darken1);
            column.Item().Text("• Please inspect the vehicle before driving off.")
                .FontSize(9).FontColor(Colors.Grey.Darken1);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Column(column =>
        {
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
            column.Item().PaddingTop(5).Text(text =>
            {
                text.Span("Thank you for choosing our car rental service!").FontSize(10).Italic();
            });
            column.Item().Text(text =>
            {
                text.Span($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(8)
                    .FontColor(Colors.Grey.Medium);
            });
        });
    }
}