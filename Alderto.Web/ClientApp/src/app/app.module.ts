import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app.routing';
import { AppComponent, AccountComponent, LoginComponent, ServerSelectComponent } from './layout';

import { AppHeaderModule, AppSidebarModule, AppFooterModule, AppBreadcrumbModule } from '@coreui/angular';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { JwtInterceptor, ErrorInterceptor } from './interceptors';
import { P404Component } from './views/error/404.component';

@NgModule({
  declarations: [
    AppComponent,
    AccountComponent,
    P404Component,
    LoginComponent,
    ServerSelectComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppHeaderModule,
    AppSidebarModule,
    AppBreadcrumbModule.forRoot(),
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
