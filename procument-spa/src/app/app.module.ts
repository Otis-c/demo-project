import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { ModalModule, BsDatepickerModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { TopBarComponent } from './nav/top-bar/top-bar.component';
import { LoginComponent } from './auth/login/login.component';
import { SideBarComponent } from './nav/side-bar/side-bar.component';
import { RegisterComponent } from './auth/register/register.component';
import { AlertifyService } from './_services/alertify.service';
import { AuthService } from './_services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { RequisitionsListComponent } from './requisitions/requisitions-list/requisitions-list.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { AdminService } from './_services/admin.service';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';
import { CreateRequisitionComponent } from './requisitions/create-requisition/create-requisition.component';
import { RequisitionsService } from './_services/requisitions.service';
import { SubmitQuoteComponent } from './requisitions/submit-quote/submit-quote.component';
import { ViewLogsComponent } from './admin/view-logs/view-logs.component';
import { HomeComponent } from './home/home.component';
import { ErrorInterceptorProvider } from './_services/error-interceptor';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    TopBarComponent,
    SideBarComponent,
    LoginComponent,
    RegisterComponent,
    RequisitionsListComponent,
    AdminPanelComponent,
    HasRoleDirective,
    RolesModalComponent,
    CreateRequisitionComponent,
    SubmitQuoteComponent,
    ViewLogsComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    })
  ],
  providers: [
    AuthService,
    AlertifyService,
    AdminService,
    RequisitionsService,
    ErrorInterceptorProvider,
  ],
  entryComponents: [
    RolesModalComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
