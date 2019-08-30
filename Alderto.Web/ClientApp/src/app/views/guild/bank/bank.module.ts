import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from "ngx-bootstrap/modal";
import { NgSelectModule } from '@ng-select/ng-select';
import { BankRoutingModule } from './bank.routing';

import { OverviewComponent } from './overview.component';

import { BankCreateComponent } from './modals/bank-create.component';
import { BankRemoveComponent } from './modals/bank-remove.component';
import { BankEditComponent } from './modals/bank-edit.component';

@NgModule({
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    NgSelectModule,
    ReactiveFormsModule,
    BankRoutingModule
  ],
  declarations: [
    OverviewComponent,
    BankCreateComponent,
    BankEditComponent,
    BankRemoveComponent
  ],
  entryComponents: [
    BankCreateComponent,
    BankEditComponent,
    BankRemoveComponent
  ],
  exports: [ModalModule]
})
export class BankModule { }
