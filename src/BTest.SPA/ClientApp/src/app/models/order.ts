import {OrderDetail} from "./order-detail";

export class Order {

  public orderDetails?: OrderDetail[];
  public totalValue:number;
  public createUtc?: Date;
  public id?: number;
  // addres, payment details ...
}
