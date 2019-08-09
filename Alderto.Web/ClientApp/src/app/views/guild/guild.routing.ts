import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OverviewComponent } from './overview.component';
import { GuildLayoutComponent } from './guild-layout.component';

const routes: Routes = [
  {
    path: '',
    component: GuildLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'overview'
      },
      {
        path: 'overview',
        component: OverviewComponent,
        data: { title: 'Overview' }
      },
      {
        path: 'bank',
        loadChildren: () => import('./bank/bank.module').then(m => m.BankModule),
        data: { title: 'Bank' }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GuildRoutingModule { }
