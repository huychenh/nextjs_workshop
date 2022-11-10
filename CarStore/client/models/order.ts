export namespace Models {
  export class Order {
    id?: string;
    productId?: string;
    productName?: string;
    buyerId?: string;
    ownerId?: string;
    price?: number;
    pictureUrl?: string;
    date?: Date;
  }
}