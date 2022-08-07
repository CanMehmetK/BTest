import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./components/home/home.component";
import {CounterComponent} from "./components/counter/counter.component";
import {FetchDataComponent} from "./components/fetch-data/fetch-data.component";
import {NoAuthGuard} from "./core/auth/guards/noAuth.guard";
import {AuthGuard} from "./core/auth/guards/auth.guard";

const routes: Routes = [

  // Redirect empty path to default route
  {path: '', pathMatch: 'full', redirectTo: 'store'},

  // Redirect signed-in user to default route
  //
  // After the user signs in, the sign-in page will redirect the user to the 'signed-in-redirect'
  // path. Below is another redirection for that path to redirect the user to the desired
  // location. This is a small convenience to keep all main routes together here on this file.
  {path: 'login-redirect', pathMatch: 'full', redirectTo: 'store'},

  // Auth routes for guests
  {
    path: '',
    canActivate: [NoAuthGuard],
    canActivateChild: [NoAuthGuard],
    children: [
      {
        path: 'login',
        loadChildren: () => import('app/modules/auth/login/login.module').then(m => m.AuthLoginInModule)
      },
      {
        path: 'register',
        loadChildren: () => import('app/modules/auth/register/register.module').then(m => m.AuthRegisterModule)
      }
    ]
  },
// Auth routes for authenticated users
  {
    path: '',
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      {
        path: 'logout',
        loadChildren: () => import('app/modules/auth/logout/logout.module').then(m => m.AuthLogoutModule)
      }
    ]
  },
  {
    path: 'store',
    loadChildren: () => import('app/modules/store/store.module').then(m => m.StoreModule)
  },
  {path: 'home', component: HomeComponent},
  {path: 'counter', component: CounterComponent},
  {path: 'fetch-data', component: FetchDataComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
