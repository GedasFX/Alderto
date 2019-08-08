import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OverviewComponent } from './overview.component';
import { FlagsComponent } from './flags.component';
import { FontAwesomeComponent } from './font-awesome.component';
import { SimpleLineIconsComponent } from './simple-line-icons.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Guild Bank'
    },
    children: [
      {
        path: '',
        redirectTo: 'overview'
      },
      {
        path: 'overview',
        component: OverviewComponent,
        data: {
          title: 'Overview'
        }
      },
      {
        path: 'flags',
        component: FlagsComponent,
        data: {
          title: 'Flags'
        }
      },
      {
        path: 'font-awesome',
        component: FontAwesomeComponent,
        data: {
          title: 'Font Awesome'
        }
      },
      {
        path: 'simple-line-icons',
        component: SimpleLineIconsComponent,
        data: {
          title: 'Simple Line Icons'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BankRoutingModule {}
