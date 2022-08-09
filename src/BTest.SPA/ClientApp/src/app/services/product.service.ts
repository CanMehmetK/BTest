import {Inject, Injectable} from '@angular/core';
import {fromEvent, map, mergeMap, Observable} from 'rxjs';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Filter} from "app/models";
import {ProductPayload} from "../models/product-payload";


@Injectable({
  providedIn: 'root'
})
export class ProductService {
  itemsUrl = '';
  httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  };

  constructor(private http: HttpClient,
              @Inject('IMAGES_URL') private _imagesUrl: string) {
  }

  getItem(id: number) {
    let params = new HttpParams();
    params.append('id', id)
    return this.http.get<ProductPayload>('api/store/products?id=' + id)
  }

  getImage(imageName: string) {
    return this.http.get(
      this._imagesUrl + imageName,
      {params: {a: 1}, responseType: 'blob'})
      .pipe(mergeMap((blobResponse) => {
        const reader = new FileReader();
        reader.readAsDataURL(blobResponse);
        return fromEvent(reader, 'load')
          .pipe(map(() =>
            (reader.result as string)));
      }))
  }

  getItems(page: number, pageSize: number, filter: Filter): Observable<ProductPayload> {
    let params = new HttpParams();

    params.set("pageNumber", page.toString())
    params.set("pageSize", pageSize.toString());

    if (filter && filter.name)
      params.set("name", filter.name)
    if (filter && filter.categories)
      filter.categories.forEach(id => {
        params = params.append("categories", id)
      });

    return this.http.get<ProductPayload>('api/store/products', {params: params})
  }


}
