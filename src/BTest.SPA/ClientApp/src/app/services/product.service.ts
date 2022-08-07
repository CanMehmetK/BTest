import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Filter, Product} from "app/models";
import {ProductPayload} from "../models/product-payload";


@Injectable({
  providedIn: 'root'
})
export class ProductService {
  itemsUrl = '';
  httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  };

  constructor(private http: HttpClient) {
  }

  getItems(page: number, pageSize: number, filter: Filter): Observable<ProductPayload> {


    let params = new HttpParams()
      .set("name", filter.name)
      .set("pageNumber", page.toString())
      .set("pageSize", pageSize.toString());
    filter.categories.forEach(id=>{
    params = params.append("categories", id)});

    return this.http.get<ProductPayload>('api/store/products', {params: params})
  }


}
