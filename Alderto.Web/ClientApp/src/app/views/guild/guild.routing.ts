import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OverviewComponent } from './overview.component';
import { GuildLayoutComponent } from './guild.layout';

const routes: Routes = [
    {
        path: '',
        component: GuildLayoutComponent,
        children: [
            {
                path: '',
                redirectTo: 'bank',
                pathMatch: 'full'
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
            },
            {
                path: 'message',
                loadChildren: () => import('./messages/message.module').then(m => m.MessageModule),
                data: { title: 'Manged Messages' }
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GuildRoutingModule { }
