import {Inject, Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {ToastrService} from 'ngx-toastr';

import {AuthService} from 'app/core/auth/auth.service';
import {AuthUtils} from 'app/core/auth/auth.utils';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private _authService: AuthService,
              private _toastr: ToastrService,
              @Inject('BASE_URL') private _baseUrl: string) {

  }

  addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({setHeaders: {Authorization: 'Bearer ' + token}})
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Clone the request object
    let newReq = req;

    // /api prefix... if request not start with http or https and start with api add server url from environment
    if (!/(http(s?)):\/\//i.test(newReq.url) && newReq.url.startsWith('api/')) {
      newReq = newReq.clone({url: (this._baseUrl + newReq.url).replace(/api\/api\//i, 'api/')});
    }


    if (this._authService.accessToken && !AuthUtils.isTokenExpired(this._authService.accessToken)) {
      newReq = newReq.clone({
        headers: newReq.headers.set('Authorization', 'Bearer ' + this._authService.accessToken)
      });
    }
    if (AuthUtils.isTokenExpired(this._authService.accessToken) && !newReq.url.includes('refresh-token')) {

    }

    // Response
    return next.handle(newReq).pipe(
      catchError((error) => {

        if (error instanceof HttpErrorResponse) {
          if (error.status === 400) {
            this._toastr.error(error.error.Message);
          } else if (error.status === 401) {
            this._authService.signInUsingToken().subscribe();
          }

        }
        return throwError(error);
      })
    );
  }
}
