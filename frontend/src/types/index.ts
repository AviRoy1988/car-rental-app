export interface Car {
  id: number;
  make: string;
  model: string;
  year: number;
  color: string;
  licensePlate: string;
  dailyRate: number;
  status: string;
  category: string;
  mileage: number;
  imageUrl?: string;
}

export interface CreateCarDto {
  make: string;
  model: string;
  year: number;
  color: string;
  vin: string;
  licensePlate: string;
  dailyRate: number;
  category: string;
  mileage: number;
  imageUrl?: string;
}

export interface Rental {
  id: number;
  customerId: number;
  customerName: string;
  carId: number;
  carDetails: string;
  startDate: string;
  endDate: string;
  actualReturnDate?: string;
  totalCost: number;
  status: string;
  notes?: string;
}

export interface CreateRentalDto {
  customerId: number;
  carId: number;
  startDate: string;
  endDate: string;
  notes?: string;
}

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  licenseNumber: string;
}

export interface LoginDto {
  username: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  email: string;
  role: string;
  expiresAt: string;
}
