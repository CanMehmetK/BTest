import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';

import {AuthService} from 'app/core/auth/auth.service';

@Component({
  selector: 'auth-sign-up',
  templateUrl: './register.component.html',
  encapsulation: ViewEncapsulation.None
})
export class AuthRegisterComponent implements OnInit {
  submitted = false;
  success: boolean = false;
  errorMessage: string = "";
  registrationForm: FormGroup;
  showAlert: boolean = false;
verificationUrl: any;


  constructor(
    private _toastr: ToastrService,
    private _authService: AuthService,
    private _formBuilder: FormBuilder,
    private _router: Router
  ) {
  }




  ngOnInit(): void {
    // Create the form
    this.registrationForm = this._formBuilder.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required],
        confirmPassword: ['', [Validators.required]]
      }
    );
  }


  onSubmit(): void {
    this.submitted = true;
    // Do nothing if the form is invalid
    if (this.registrationForm.invalid) {
      return;
    }

    // Disable the form
    this.registrationForm.disable();

    // Hide the alert
    this.showAlert = false;

    // Sign up
    this._authService.signUp(this.registrationForm.value)
      .subscribe(
        (response) => {
          this._toastr.success("Registration completed.");

          this.verificationUrl = response.data;
          // Navigate to the confirmation required page
          // this._router.navigateByUrl('/confirmation-required');
        },
        (response) => {

          // Re-enable the form
          this.registrationForm.enable();

          // Reset the form


          // Set the alert

          // Show the alert
          this.showAlert = true;
        }
      );
  }
}
