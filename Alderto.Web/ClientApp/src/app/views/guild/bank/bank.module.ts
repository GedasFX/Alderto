import { NgModule } from '@angular/core';

import { OverviewComponent } from './overview.component';

import { BankRoutingModule } from './bank.routing';

@NgModule({
  imports: [
    BankRoutingModule
  ],
  declarations: [
    OverviewComponent
  ]
})
export class BankModule { }
