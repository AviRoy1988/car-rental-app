import React, { useState, useEffect } from "react";
import { rentalService } from "../services/rentalService";
import { RentalDto } from "../types/rental.types";
import "./RentalList.css";

interface RentalListProps {
  activeOnly?: boolean;
}

const RentalList: React.FC<RentalListProps> = ({ activeOnly = false }) => {
  const [rentals, setRentals] = useState<RentalDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchRentals();
  }, [activeOnly]);

  const fetchRentals = async () => {
    setLoading(true);
    setError(null);

    try {
      const data = activeOnly
        ? await rentalService.getActiveRentals()
        : await rentalService.getAllRentals();
      setRentals(data);
    } catch (err: any) {
      setError("Failed to fetch rentals");
    } finally {
      setLoading(false);
    }
  };

  const handleDownloadInvoice = async (bookingNumber: string) => {
    try {
      const blob = await rentalService.downloadInvoice(bookingNumber);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.download = `Invoice_${bookingNumber}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (err) {
      alert("Failed to download invoice");
    }
  };

  if (loading) {
    return (
      <div className="rental-list-container">
        <p>Loading rentals...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="rental-list-container">
        <div className="error-message">{error}</div>
      </div>
    );
  }

  return (
    <div className="rental-list-container">
      <div className="list-header">
        <h2>{activeOnly ? "Active Rentals" : "All Rentals"}</h2>
        <button onClick={fetchRentals} className="refresh-button">
          Refresh
        </button>
      </div>

      {rentals.length === 0 ? (
        <p className="no-rentals">No rentals found.</p>
      ) : (
        <div className="rentals-grid">
          {rentals.map((rental) => (
            <div key={rental.bookingNumber} className="rental-card">
              <div className="rental-card-header">
                <h3>{rental.registrationNumber}</h3>
                <span className={`status-badge ${rental.status.toLowerCase()}`}>
                  {rental.status}
                </span>
              </div>

              <div className="rental-card-body">
                <div className="rental-info">
                  <label>Booking #:</label>
                  <span>{rental.bookingNumber}</span>
                </div>
                <div className="rental-info">
                  <label>Category:</label>
                  <span>{rental.category}</span>
                </div>
                <div className="rental-info">
                  <label>Customer SSN:</label>
                  <span>{rental.customerSocialSecurityNumber}</span>
                </div>
                <div className="rental-info">
                  <label>Pickup:</label>
                  <span>
                    {new Date(rental.pickupDateTime).toLocaleString()}
                  </span>
                </div>
                <div className="rental-info">
                  <label>Pickup Meter:</label>
                  <span>{rental.pickupMeterReading} km</span>
                </div>

                {rental.returnDateTime && (
                  <>
                    <div className="rental-info">
                      <label>Return:</label>
                      <span>
                        {new Date(rental.returnDateTime).toLocaleString()}
                      </span>
                    </div>
                    <div className="rental-info">
                      <label>Return Meter:</label>
                      <span>{rental.returnMeterReading} km</span>
                    </div>
                    <div className="rental-info">
                      <label>Duration:</label>
                      <span>
                        {rental.numberOfDays} days | {rental.numberOfKm} km
                      </span>
                    </div>
                    <div className="rental-info price">
                      <label>Total Price:</label>
                      <span className="price-value">
                        ${rental.calculatedPrice?.toFixed(2)}
                      </span>
                    </div>
                  </>
                )}
              </div>

              {rental.status === "Completed" && (
                <div className="rental-card-footer">
                  <button
                    onClick={() => handleDownloadInvoice(rental.bookingNumber)}
                    className="download-button"
                  >
                    Download Invoice
                  </button>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default RentalList;
