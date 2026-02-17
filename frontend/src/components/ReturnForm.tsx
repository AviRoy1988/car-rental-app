import React, { useState } from "react";
import { rentalService } from "../services/rentalService";
import { ReturnRentalRequest, RentalDto } from "../types/rental.types";
import "./ReturnForm.css";

const ReturnForm: React.FC = () => {
  const [bookingNumber, setBookingNumber] = useState("");
  const [rental, setRental] = useState<RentalDto | null>(null);
  const [formData, setFormData] = useState<ReturnRentalRequest>({
    returnDateTime: new Date().toISOString().slice(0, 16),
    returnMeterReading: 0,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<RentalDto | null>(null);

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setRental(null);
    setSuccess(null);

    try {
      const result =
        await rentalService.getRentalByBookingNumber(bookingNumber);
      if (!result) {
        setError("Rental not found");
      } else if (result.status !== "Active") {
        setError("This rental is already completed");
      } else {
        setRental(result);
        setFormData({
          ...formData,
          returnMeterReading: result.pickupMeterReading, // Set minimum
        });
      }
    } catch (err: any) {
      setError("Failed to fetch rental details");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!rental) return;

    setLoading(true);
    setError(null);

    try {
      const result = await rentalService.registerReturn(
        bookingNumber,
        formData,
      );
      setSuccess(result);
      setRental(null);
      setBookingNumber("");
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to register return");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="return-form-container">
      <h2>Register Car Return</h2>

      {error && <div className="error-message">{error}</div>}
      {success && (
        <div className="success-message">
          <h3>Return Registered Successfully!</h3>
          <p>Booking Number: {success.bookingNumber}</p>
          <p>
            Total Price: <strong>${success.calculatedPrice?.toFixed(2)}</strong>
          </p>
          <p>
            Days: {success.numberOfDays} | Distance: {success.numberOfKm} km
          </p>
        </div>
      )}

      {!rental && (
        <form onSubmit={handleSearch} className="search-form">
          <div className="form-group">
            <label htmlFor="bookingNumber">Booking Number *</label>
            <input
              type="text"
              id="bookingNumber"
              value={bookingNumber}
              onChange={(e) => setBookingNumber(e.target.value)}
              required
              placeholder="Enter booking number"
            />
          </div>
          <button type="submit" disabled={loading} className="search-button">
            {loading ? "Searching..." : "Search Rental"}
          </button>
        </form>
      )}

      {rental && (
        <div className="rental-details">
          <h3>Rental Details</h3>
          <div className="details-grid">
            <div>
              <strong>Registration:</strong> {rental.registrationNumber}
            </div>
            <div>
              <strong>Category:</strong> {rental.category}
            </div>
            <div>
              <strong>Customer SSN:</strong>{" "}
              {rental.customerSocialSecurityNumber}
            </div>
            <div>
              <strong>Pickup Date:</strong>{" "}
              {new Date(rental.pickupDateTime).toLocaleString()}
            </div>
            <div>
              <strong>Pickup Meter:</strong> {rental.pickupMeterReading} km
            </div>
          </div>

          <form onSubmit={handleSubmit} className="return-form">
            <div className="form-group">
              <label htmlFor="returnDateTime">Return Date/Time *</label>
              <input
                type="datetime-local"
                id="returnDateTime"
                value={formData.returnDateTime}
                onChange={(e) =>
                  setFormData({ ...formData, returnDateTime: e.target.value })
                }
                required
                min={rental.pickupDateTime.slice(0, 16)}
              />
            </div>

            <div className="form-group">
              <label htmlFor="returnMeterReading">
                Return Meter Reading (km) *
              </label>
              <input
                type="number"
                id="returnMeterReading"
                value={formData.returnMeterReading}
                onChange={(e) =>
                  setFormData({
                    ...formData,
                    returnMeterReading: parseInt(e.target.value),
                  })
                }
                required
                min={rental.pickupMeterReading}
              />
            </div>

            <div className="button-group">
              <button
                type="button"
                onClick={() => setRental(null)}
                className="cancel-button"
              >
                Cancel
              </button>
              <button
                type="submit"
                disabled={loading}
                className="submit-button"
              >
                {loading ? "Processing..." : "Register Return"}
              </button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
};

export default ReturnForm;
