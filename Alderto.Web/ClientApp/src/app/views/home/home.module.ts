import { NgModule } from '@angular/core';

import { NewsComponent } from './news.component';

import { HomeRoutingModule } from './home.routing';
import { HomeLayoutComponent } from "./home.layout";

@NgModule({
  imports: [
    HomeRoutingModule
  ],
  declarations: [
    HomeLayoutComponent,
    NewsComponent
  ]
})
export class HomeModule { }
