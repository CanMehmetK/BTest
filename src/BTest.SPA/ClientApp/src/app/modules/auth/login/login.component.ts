import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {Location} from '@angular/common'
import {AuthService} from "app/core/auth/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({});
  loading: boolean = false;
  submitted: boolean = false;
  error: string = '';
  confirmRequired=false;

  constructor(private _formBuilder: FormBuilder,
              private _toastr: ToastrService,
              private _authService: AuthService,
              private _location: Location,
              public _activatedRoute: ActivatedRoute,
              public _router: Router
  ) {

    const routeSnap = _activatedRoute.snapshot;
    const code = routeSnap.params.code || routeSnap.queryParams['code'];
    const email = routeSnap.params.email || routeSnap.queryParams['email'];
    if (code && email) {
      this.confirmEmail(code, email);
    }
  }

  ngOnInit() {
    this.loginForm = this._formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.loginForm?.invalid)
      return;

    this.loading = true;
    this._authService.signIn(this.loginForm.value)
      .subscribe({
        next: () => {
          const returnUrl = this._activatedRoute.snapshot.queryParams['returnUrl'] || 'login-redirect';
          this._router.navigate([returnUrl]);
        },
        error: error => {
          this.error = error.error.Message;
          this.loading = false;
          if(this.error.indexOf('Account Not Confirmed for')>-1) this.confirmRequired=true;
        }
      });
  }

  confirmEmail(code: string, email: string): void {
    this._authService.confirmEmail(code, email)
      .subscribe((response) => {
        if (response.data == email) {

          this._location.replaceState('/login');
          this._toastr.success(response.message);
        }
      });
  }

}
