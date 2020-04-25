import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { P404Component } from './views/error/404.component';

import { AuthGuard } from './auth.guard';

const routes: Routes = [
  {
    // Index.
    path: '',
    loadChildren: () => import('./views/home/home.module').then(m => m.HomeModule)
  },
  {
    path: 'guild/:id',
    loadChildren: () => import('./views/guild/guild.module').then(m => m.GuildModule),
    data: { title: 'Guild' },
    // canActivate: [AuthGuard]
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
