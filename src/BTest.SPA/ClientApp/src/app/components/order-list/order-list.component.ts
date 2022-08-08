import { Component, OnInit } from '@angular/core';
import {StoreService} from "../../services/store.service";

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit {

  constructor(public _storeService:StoreService) { }

  ngOnInit(): void {
  }

}
