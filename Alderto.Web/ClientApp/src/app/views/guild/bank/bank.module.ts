import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from "ngx-bootstrap";

import { BankRoutingModule } from './bank.routing';

import { OverviewComponent } from './overview.component';

import { BankCreateComponent } from './modals/bank-create.component';

@NgModule({
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    ReactiveFormsModule,
    BankRoutingModule
  ],
  declarations: [
    OverviewComponent,
    BankCreateComponent
  ],
  exports: [ModalModule]
})
export class BankModule { }
