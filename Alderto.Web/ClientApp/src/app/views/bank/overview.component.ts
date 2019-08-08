import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ViewBaseComponent } from '../view-base.component';

@Component({
  templateUrl: 'overview.component.html'
})
export class OverviewComponent extends ViewBaseComponent {
  constructor(route: ActivatedRoute) { super(route); }
}
