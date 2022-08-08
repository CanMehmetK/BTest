import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {Cart, Category, Filter, Order, OrderDetail, Product} from "../models";
import {User} from "app/models/user";
import {HttpClient} from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class StoreService {
  public pageSize: number = 3;
  public readonly _pageSizeSubject = new Subject<number>();
  public pageSizeChanges$ = this._pageSizeSubject.asObservable();

  private readonly _products = new BehaviorSubject<Product[]>([]);
  private readonly _categories = new BehaviorSubject<Category[]>([]);
  readonly categories$ = this._categories.asObservable();

  private readonly _page = new BehaviorSubject<number>(1);
  readonly page$ = this._page.asObservable();

  private readonly _count = new BehaviorSubject<number>(1);
  readonly count$ = this._count.asObservable();

  private readonly _filter = new BehaviorSubject<Filter>({name: "", categories: []});
  readonly filter$ = this._filter.asObservable();

  private readonly _cart = new BehaviorSubject<Cart>(new Cart(localStorage.getItem('cart') || ''));
  readonly cart$ = this._cart.asObservable();

  private readonly _user
    = new BehaviorSubject<User | null>(
    (sessionStorage.getItem('user') === null) ? null : JSON.parse(sessionStorage.getProduct('user') ?? "")
  );
  readonly user$ = this._user.asObservable();

  private readonly _deliveryAddress = new BehaviorSubject<number>(-1);
  readonly deliveryAddress$ = this._deliveryAddress.asObservable();
  private readonly _order = new BehaviorSubject<Order>(new Order());
  readonly order$ = this._order.asObservable();

  private readonly _myOrders = new BehaviorSubject<Order[]>([]);


  constructor(private _httpClient: HttpClient) {
    this.getAllCategories();
    this.getMyOrders();
  }

  get myOrders$() {
    return this._myOrders.asObservable();
  }

  get categories(): Category[] {
    return this._categories.getValue();
  }

  set categories(val: Category[]) {
    this._categories.next(val);
  }

  get products(): Product[] {
    return this._products.getValue();
  }

  set products(val: Product[]) {
    this._products.next(val);
  }

  get page(): number {
    return this._page.getValue();
  }

  set page(val: number) {
    this._page.next(val);
  }

  get count(): number {
    return this._count.getValue();
  }

  set count(val: number) {
    this._count.next(val);
  }

  get filter(): Filter {
    return this._filter.getValue();
  }

  set filter(val: Filter) {
    this._filter.next(val);
  }

  get cart(): Cart {
    return this._cart.getValue();
  }

  set cart(val: Cart) {
    this._cart.next(val);
  }

  get user(): User | null {
    return this._user.getValue();
  }

  set user(val: User | null) {
    this._user.next(val);
  }

  get deliveryAddress(): number {
    return this._deliveryAddress.getValue();
  }

  set deliveryAddress(val: number) {
    this._deliveryAddress.next(val);
  }

  get order(): Order {
    return this._order.getValue();
  }

  set order(val: Order) {
    this._order.next(val);
  }

  createOrder(): Observable<any> {
    let order: Order = new Order();
    order.totalValue = this.cart.totalValue;
    order.orderDetails = this.cart.cartItems.map((cartItem) => {
      let orderDetail: OrderDetail = new OrderDetail();
      orderDetail.productId = cartItem.product.id;
      orderDetail.quantity = cartItem.quantity;
      orderDetail.unitPrice = cartItem.product.price;
      orderDetail.totalPrice = this.round(cartItem.quantity * cartItem.product.price)
      return orderDetail;
    });
    return this._httpClient.post('api/store/create-order', order);

  }

  round(value: number, direction: string = 'down', digits: number = 2): any {
    const round = 'down' === direction ? Math.floor : Math.ceil;
    return round(value * (10 ** digits)) / (10 ** digits);
  }

  private getAllCategories() {
    this._httpClient.get('api/store/categories')
      .subscribe((response: any) => {
        this._categories.next(response.data);
      })
  }

  public getMyOrders() {
    this._httpClient.post('api/store/my-orders', {})
      .subscribe((response: any) => {
      this._myOrders.next(response.data);
      })
  }
}
