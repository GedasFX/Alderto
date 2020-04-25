import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app.routing';
import { AppComponent, AccountComponent, ServerSelectComponent } from './layout';

import { AppHeaderModule, AppBreadcrumbModule, AppSidebarModule, AppFooterModule } from '@coreui/angular';
import { BsDropdownModule } from 'ngx-bootstrap';
import { TokenInterceptor } from './interceptors';
import { P404Component } from './views/error/404.component';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    AccountComponent,
    P404Component,
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
    BsDropdownModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
