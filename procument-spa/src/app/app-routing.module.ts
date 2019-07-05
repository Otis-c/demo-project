import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { RequisitionsListComponent } from './requisitions/requisitions-list/requisitions-list.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { AuthGuard } from './_guards/auth-guard';
import { CreateRequisitionComponent } from './requisitions/create-requisition/create-requisition.component';
import { SubmitQuoteComponent } from './requisitions/submit-quote/submit-quote.component';
import { ViewLogsComponent } from './admin/view-logs/view-logs.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: LoginComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'requisitions', component: RequisitionsListComponent },
      { path: 'create-requisition', component: CreateRequisitionComponent },
      { path: 'submitQuote/:id', component: SubmitQuoteComponent },
      { path: 'admin', component: AdminPanelComponent },
      { path: 'logs', component: ViewLogsComponent }

    ]
  },
  { path: '**', redirectTo: '' }

];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
