import {Component} from '@angular/core';
import {UserService} from "../../core/auth/user.service";
import {AuthService} from "../../core/auth/auth.service";
import {AuthUtils} from "../../core/auth/auth.utils";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(public _userService: UserService,
              public _authService: AuthService) {
    // setInterval(() => {
    //   console.log(AuthUtils.isTokenExpired(this._authService.accessToken));
    // }, 5000)
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
