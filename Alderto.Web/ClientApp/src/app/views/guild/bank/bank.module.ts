import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from "ngx-bootstrap/modal";
import { NgSelectModule } from '@ng-select/ng-select';
import { BankRoutingModule } from './bank.routing';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

import { OverviewComponent } from './overview.component';

import { BankCreateComponent } from './modals/bank-create.component';
import { BankRemoveComponent } from './modals/bank-remove.component';
import { BankEditComponent } from './modals/bank-edit.component';
import { BankItemsCreateComponent } from './modals/bank-items-create.component';
import { BankItemsDetailsComponent } from './modals/bank-items-details.component';

@NgModule({
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    NgSelectModule,
    ReactiveFormsModule,
    BankRoutingModule,
    TooltipModule.forRoot()
  ],
  declarations: [
    OverviewComponent,
    BankCreateComponent,
    BankEditComponent,
    BankRemoveComponent,
    BankItemsCreateComponent,
    BankItemsDetailsComponent
  ],
  entryComponents: [
    BankCreateComponent,
    BankEditComponent,
    BankRemoveComponent,
    BankItemsCreateComponent,
    BankItemsDetailsComponent
  ],
  exports: [ModalModule]
})
export class BankModule { }
