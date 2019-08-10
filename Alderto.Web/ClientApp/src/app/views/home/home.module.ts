import { NgModule } from '@angular/core';

import { NewsComponent } from './news.component';

import { HomeRoutingModule } from './home.routing';

@NgModule({
  imports: [
    HomeRoutingModule
  ],
  declarations: [
    NewsComponent
  ]
})
export class HomeModule { }
