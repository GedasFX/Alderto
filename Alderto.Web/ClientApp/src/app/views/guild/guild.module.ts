import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { OverviewComponent } from './overview.component';

import { GuildRoutingModule } from './guild.routing';
import { AppSidebarModule, AppBreadcrumbModule } from '@coreui/angular';
import { GuildLayoutComponent } from './guild.layout';

@NgModule({
  imports: [
    CommonModule,
    GuildRoutingModule
  ],
  declarations: [
    GuildLayoutComponent,
    OverviewComponent
  ]
})
export class GuildModule { }
