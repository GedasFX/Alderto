import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { P404Component } from './views/error/404.component';

import { AuthGuard } from './auth.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '666666/bank',
    //canActivate: [AuthGuard]
  },
  {
    path: ':id',
    pathMatch: 'full',
    redirectTo: ':id/bank'
  },
  {
    path: ':id/bank',
    loadChildren: () => import('./views/bank/bank.module').then(m => m.BankModule),
    //canActivate: [AuthGuard]
  },
  {
    path: '**',
    component: P404Component
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
