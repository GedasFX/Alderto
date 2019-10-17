import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from "ngx-bootstrap/modal";
import { NgSelectModule } from '@ng-select/ng-select';
import { MessageRoutingModule } from './message.routing';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

import { OverviewComponent } from './overview.component';

import { MessageCreateComponent } from './modals/message-create.component';
import { MessageRemoveComponent } from './modals/message-remove.component';
import { MessageEditComponent } from './modals/message-edit.component';

@NgModule({
    imports: [
        CommonModule,
        ModalModule.forRoot(),
        NgSelectModule,
        ReactiveFormsModule,
        MessageRoutingModule,
        TooltipModule.forRoot()
    ],
    declarations: [
        OverviewComponent,
        MessageCreateComponent,
        MessageEditComponent,
        MessageRemoveComponent
    ],
    entryComponents: [
        MessageCreateComponent,
        MessageEditComponent,
        MessageRemoveComponent
    ],
    exports: [ModalModule]
})
export class MessageModule { }
