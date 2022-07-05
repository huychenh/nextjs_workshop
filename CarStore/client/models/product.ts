export namespace Models {
  export class Product {
    name?: string;
    price?: number;
    brand?: string;
    model?: string;
    transmission?: string;
    madeIn?: string;
    seatingCapacity?: number;
    kmDriven?: number;
    year?: number;
    fuelType?: string;
    category?: string;
    color?: string;
    description?: string;
    hasInstallment?: boolean;
    verified?: boolean;
    ownerName?: string;
    id?: string;
    active?: boolean;
    created?: string;
    updated?: string;
    totalPages?: number; // todo: refactor this
  }
}