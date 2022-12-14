import {Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import {Observable, of, switchMap} from 'rxjs';
import {AuthService} from 'app/core/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {

  constructor(
    private _authService: AuthService,
    private _router: Router
  ) {
  }


  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const redirectUrl = state.url === '/sign-out' ? '/' : state.url;
    return this._check(redirectUrl,route.data.roles);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const redirectUrl = state.url === '/sign-out' ? '/' : state.url;
    return this._check(redirectUrl,childRoute.data.roles);
  }


  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this._check('/');
  }


  private _check(redirectURL: string,roles:string[]=undefined): Observable<boolean> {
    // Check the authentication status
    return this._authService.check(roles)
      .pipe(
        switchMap((authenticated) => {

          // If the user is not authenticated...
          if (!authenticated) {
            // Redirect to the login page
            this._router.navigate(['login'], {queryParams: {redirectURL}});

            // Prevent the access
            return of(false);
          }

          // Allow the access
          return of(true);
        })
      );
  }
}
