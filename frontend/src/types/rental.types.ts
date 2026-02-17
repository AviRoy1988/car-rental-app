// Types matching the backend DTOs
export enum CarCategory {
  SmallCar = 0,
  Combi = 1,
  Truck = 2,
}

export enum RentalStatus {
  Active = 0,
  Completed = 1,
}

export interface RentalDto {
  bookingNumber: string;
  registrationNumber: string;
  customerSocialSecurityNumber: string;
  emailAddress: string;
  category: string;
  pickupDateTime: string;
  pickupMeterReading: number;
  returnDateTime?: string;
  returnMeterReading?: number;
  calculatedPrice?: number;
  status: string;
  numberOfDays?: number;
  numberOfKm?: number;
}

export interface PickupRentalRequest {
  registrationNumber: string;
  customerSocialSecurityNumber: string;
  emailAddress: string;
  category: CarCategory;
  pickupDateTime: string;
  pickupMeterReading: number;
}

export interface ReturnRentalRequest {
  returnDateTime: string;
  returnMeterReading: number;
}
