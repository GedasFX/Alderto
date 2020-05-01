import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { OverviewComponent } from './overview.component';

import { GuildRoutingModule } from './guild.routing';
import { GuildLayoutComponent } from './guild.layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    GuildRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    GuildLayoutComponent,
    OverviewComponent
  ]
})
export class GuildModule { }
