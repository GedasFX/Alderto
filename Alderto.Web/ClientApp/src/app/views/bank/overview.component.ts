import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  templateUrl: 'overview.component.html'
})
export class OverviewComponent {

  constructor(private readonly httpClient: HttpClient) { }
}
