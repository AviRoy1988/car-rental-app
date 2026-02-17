import axios from "axios";
import {
  RentalDto,
  PickupRentalRequest,
  ReturnRentalRequest,
} from "../types/rental.types";

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5100";

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const rentalService = {
  // Register car pickup
  registerPickup: async (request: PickupRentalRequest): Promise<RentalDto> => {
    const response = await api.post<RentalDto>("/api/rentals/pickup", request);
    return response.data;
  },

  // Register car return
  registerReturn: async (
    bookingNumber: string,
    request: ReturnRentalRequest,
  ): Promise<RentalDto> => {
    const response = await api.post<RentalDto>(
      `/api/rentals/${bookingNumber}/return`,
      request,
    );
    return response.data;
  },

  // Get rental by booking number
  getRentalByBookingNumber: async (
    bookingNumber: string,
  ): Promise<RentalDto | null> => {
    try {
      const response = await api.get<RentalDto>(
        `/api/rentals/${bookingNumber}`,
      );
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  // Get all rentals
  getAllRentals: async (): Promise<RentalDto[]> => {
    const response = await api.get<RentalDto[]>("/api/rentals");
    return response.data;
  },

  // Get active rentals only
  getActiveRentals: async (): Promise<RentalDto[]> => {
    const response = await api.get<RentalDto[]>("/api/rentals/active");
    return response.data;
  },

  // Download invoice
  downloadInvoice: async (bookingNumber: string): Promise<Blob> => {
    const response = await api.get(`/api/invoice/${bookingNumber}`, {
      responseType: "blob",
    });
    return response.data;
  },
};
