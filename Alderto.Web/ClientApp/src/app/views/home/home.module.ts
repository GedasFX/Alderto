import { NgModule } from '@angular/core';

import { NewsComponent } from './news.component';
import { DocumentationComponent } from './documentation.component';

import { HomeRoutingModule } from './home.routing';
import { HomeLayoutComponent } from './home.layout';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';

@NgModule({
  imports: [
    CommonModule,
    HomeRoutingModule,
    TabsModule.forRoot()
  ],
  declarations: [
    HomeLayoutComponent,
    DocumentationComponent,
    NewsComponent
  ]
})
export class HomeModule { }
