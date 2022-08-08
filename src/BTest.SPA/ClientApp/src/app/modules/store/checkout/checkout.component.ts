import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../../core/auth/auth.service";
import {StoreService} from "../../../services/store.service";
import {ToastrService} from 'ngx-toastr';
import {Router} from "@angular/router";

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  aggrement: boolean = false;
  posting: boolean=false;
  orderStepMessage;


  constructor(private _router: Router,
              private _toastr: ToastrService,
              public _authService: AuthService,
              private _storeService: StoreService) {
  }

  ngOnInit(): void {
  }

  sendOrder() {
    this.posting=true;
    this.orderStepMessage = [];
    setTimeout(() => {
      this.orderStepMessage.push("Getting payment")
      setTimeout(() => {
        this.orderStepMessage[0] = "Getting payment [Done]"
        this.orderStepMessage.push("Creating Order")


        this._storeService.createOrder().subscribe((response) => {
          this.posting=false;
          this.orderStepMessage[1] = "Creating Order [Done] :)";
          this._toastr.success(`Your Order #${response.data.id} created and email sent to you`);
          this._storeService.cart.emptyCart();
          this._storeService.getMyOrders();
          setTimeout(() => {
            this._router.navigate(['store']).then();
          }, 500)
        });

      }, 500);
    }, 500);
  }

  setMessage() {
  }
}
