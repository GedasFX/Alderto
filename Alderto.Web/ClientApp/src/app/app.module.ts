import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent, AccountComponent } from './layout';

import { AppHeaderModule, AppSidebarModule, AppFooterModule } from '@coreui/angular';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { JwtInterceptor, ErrorInterceptor } from './interceptors';

@NgModule({
  declarations: [
    AppComponent,
    AccountComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppHeaderModule,
    AppSidebarModule,
    AppFooterModule,
    HttpClientModule,
    BsDropdownModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
