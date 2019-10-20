import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from "ngx-bootstrap/modal";
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgSelectModule } from '@ng-select/ng-select';
import { MessageRoutingModule } from './message.routing';

import { OverviewComponent } from './overview.component';

import { MessageCreateComponent } from './modals/message-create.component';
import { MessageRemoveComponent } from './modals/message-remove.component';
import { MessageEditComponent } from './modals/message-edit.component';
import { NgPipesModule } from 'ngx-pipes';

@NgModule({
    imports: [
        CommonModule,
        ModalModule.forRoot(),
        NgPipesModule,
        NgSelectModule,
        ReactiveFormsModule,
        MessageRoutingModule,
        TabsModule.forRoot()
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
