import {Component, OnInit} from '@angular/core';
import {Filter} from "app/models";
import {StoreService} from "app/services/store.service";


@Component({
  selector: 'product-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {

  categories = [];

  tempFilter: Filter = {name: "", categories: []};

  constructor(
    public _storeService: StoreService
  ) {
    _storeService.categories$.subscribe(t => this.categories = t);

  }

  ngOnInit(): void {
    this.tempFilter = this._storeService.filter;
    this.categories = this.categories.map(cat => ({
      name: cat.name,
      selected: (this.tempFilter.categories.includes(cat.id))
    }));
  }

  onChange(): void {
    this.tempFilter.categories = this.categories.filter(c => c.selected).map(cc => cc.id);
  }

  onFilterChanged(): void {
    this._storeService.filter = this.tempFilter;
  }
}
