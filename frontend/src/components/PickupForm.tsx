import React, { useState } from "react";
import { rentalService } from "../services/rentalService";
import {
  PickupRentalRequest,
  CarCategory,
  RentalDto,
} from "../types/rental.types";
import "./PickupForm.css";

const PickupForm: React.FC = () => {
  const [formData, setFormData] = useState<PickupRentalRequest>({
    registrationNumber: "",
    customerSocialSecurityNumber: "",
    emailAddress: "",
    category: CarCategory.SmallCar,
    pickupDateTime: new Date().toISOString().slice(0, 16),
    pickupMeterReading: 0,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<RentalDto | null>(null);

  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);

    try {
      const result = await rentalService.registerPickup(formData);
      setSuccess(result);
      // Reset form
      setFormData({
        registrationNumber: "",
        customerSocialSecurityNumber: "",
        emailAddress: "",
        category: CarCategory.SmallCar,
        pickupDateTime: new Date().toISOString().slice(0, 16),
        pickupMeterReading: 0,
      });
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to register pickup");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="pickup-form-container">
      <h2>Register Car Pickup</h2>

      {error && <div className="error-message">{error}</div>}
      {success && (
        <div className="success-message">
          <h3>Pickup Registered Successfully!</h3>
          <p>
            Booking Number: <strong>{success.bookingNumber}</strong>
          </p>
          <p>Registration: {success.registrationNumber}</p>
        </div>
      )}

      <form onSubmit={handleSubmit} className="pickup-form">
        <div className="form-group">
          <label htmlFor="registrationNumber">Registration Number *</label>
          <input
            type="text"
            id="registrationNumber"
            value={formData.registrationNumber}
            onChange={(e) =>
              setFormData({ ...formData, registrationNumber: e.target.value })
            }
            required
            maxLength={20}
          />
        </div>

        <div className="form-group">
          <label htmlFor="customerSSN">Customer SSN *</label>
          <input
            type="text"
            id="customerSSN"
            value={formData.customerSocialSecurityNumber}
            onChange={(e) =>
              setFormData({
                ...formData,
                customerSocialSecurityNumber: e.target.value,
              })
            }
            required
            maxLength={20}
          />
        </div>

        <div className="form-group">
          <label htmlFor="emailAddress">Email Address *</label>
          <input
            type="email"
            id="emailAddress"
            value={formData.emailAddress}
            onChange={(e) =>
              setFormData({ ...formData, emailAddress: e.target.value })
            }
            required
            placeholder="customer@example.com"
          />
        </div>

        <div className="form-group">
          <label htmlFor="category">Car Category *</label>
          <select
            id="category"
            value={formData.category}
            onChange={(e) =>
              setFormData({
                ...formData,
                category: parseInt(e.target.value) as CarCategory,
              })
            }
            required
          >
            <option value={CarCategory.SmallCar}>Small Car</option>
            <option value={CarCategory.Combi}>Combi</option>
            <option value={CarCategory.Truck}>Truck</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="pickupDateTime">Pickup Date/Time *</label>
          <input
            type="datetime-local"
            id="pickupDateTime"
            value={formData.pickupDateTime}
            onChange={(e) =>
              setFormData({ ...formData, pickupDateTime: e.target.value })
            }
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="pickupMeterReading">
            Pickup Meter Reading (km) *
          </label>
          <input
            type="number"
            id="pickupMeterReading"
            value={formData.pickupMeterReading}
            onChange={(e) =>
              setFormData({
                ...formData,
                pickupMeterReading: parseInt(e.target.value),
              })
            }
            required
            min="0"
          />
        </div>

        <button type="submit" disabled={loading} className="submit-button">
          {loading ? "Registering..." : "Register Pickup"}
        </button>
      </form>
    </div>
  );
};

export default PickupForm;
