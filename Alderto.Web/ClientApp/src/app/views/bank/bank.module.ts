import { NgModule } from '@angular/core';

import { OverviewComponent } from './overview.component';
import { FlagsComponent } from './flags.component';
import { FontAwesomeComponent } from './font-awesome.component';
import { SimpleLineIconsComponent } from './simple-line-icons.component';

import { BankRoutingModule } from './bank.routing';

@NgModule({
  imports: [
    BankRoutingModule
  ],
  declarations: [
    OverviewComponent,
    FlagsComponent,
    FontAwesomeComponent,
    SimpleLineIconsComponent
  ]
})
export class BankModule { }
