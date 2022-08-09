import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, Observable, of, switchMap, throwError} from 'rxjs';
import {AuthUtils} from 'app/core/auth/auth.utils';
import {UserService} from 'app/core/auth/user.service';

@Injectable()
export class AuthService {
  private _authenticated: boolean = false;

  constructor(
    private _httpClient: HttpClient,
    private _userService: UserService
  ) {
  }

  get loggedIn(): boolean {
    const accessToken = localStorage.getItem('accessToken');
    return ( accessToken != undefined && accessToken != '' && accessToken !=null);
  }

  get accessToken(): string {
    return localStorage.getItem('accessToken') ?? '';
  }


  set accessToken(token: string) {
    localStorage.setItem('accessToken', token);
  }

  get refreshToken(): string {
    return localStorage.getItem('refreshToken');
  }

  set refreshToken(refreshToken: string) {
    localStorage.setItem('refreshToken', refreshToken);
  }

  forgotPassword(email: string): Observable<any> {
    return this._httpClient.post('api/auth/forgot-password', email);
  }


  resetPassword(password: string): Observable<any> {
    return this._httpClient.post('api/auth/reset-password', password);
  }


  signIn(credentials: { email: string; password: string }): Observable<any> {
    // Throw error, if the user is already logged in
    if (this._authenticated) {
      return throwError('User is already logged in.');
    }

    return this._httpClient.post('api/auth/login', credentials).pipe(
      switchMap((response: any) => {

        // Store the access token in the local storage
        this.accessToken = response.data.accessToken;
        this.refreshToken = response.data.refreshToken;
        // Set the authenticated flag to true
        this._authenticated = true;

        // Store the user on the user service
        this._userService.user = {
          id: response.data.id,
          email: response.data.email,
          name: response.data.userName
        };

        // Return a new observable with the response
        return of(response);
      })
    );
  }

  signInUsingToken(): Observable<any> {
    // Renew token
    return this._httpClient.post('api/auth/refresh-token', {
      accessToken: this.accessToken,
      refreshToken : this.refreshToken
    }).pipe(
      catchError(() =>
        // Return false
        of(false)
      ),
      switchMap((response: any) => {
        this.accessToken = response.data.accessToken;
        this.refreshToken = response.data.refreshToken;
        // Set the authenticated flag to true
        this._authenticated = true;

        // Store the user on the user service
        this._userService.user = {
          id: response.data.id,
          email: response.data.email,
          name: response.data.userName
        };

        // Return true
        return of(true);
      })
    );
  }

  signOut(): Observable<any> {
    // Remove the access token from the local storage
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    // Set the authenticated flag to false
    this._authenticated = false;

    // Return the observable
    return of(true);
  }

  signUp(user: { name: string; email: string; password: string; company: string }): Observable<any> {
    return this._httpClient.post('api/auth/register', user);
  }


  unlockSession(credentials: { email: string; password: string }): Observable<any> {
    return this._httpClient.post('api/auth/unlock-session', credentials);
  }


  check(roles:string[]=undefined): Observable<boolean> {
    console.log(roles)
    // Check if the user is logged in
    if (this._authenticated) {
      return of(true);
    }

    // Check the access token availability
    if (!this.accessToken) {
      return of(false);
    }

    // Check the access token expire date
    if (AuthUtils.isTokenExpired(this.accessToken)) {
      return of(false);
    }

    // If the access token exists and it didn't expire, sign in using it
    return this.signInUsingToken();
  }

  confirmEmail(code: string, email: string) {
    return this._httpClient.post<any>('api/auth/confirm-email', {code: code, email: email});

  }
}
