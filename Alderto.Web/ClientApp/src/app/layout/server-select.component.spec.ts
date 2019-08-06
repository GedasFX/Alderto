import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerSelectComponent } from './server-select.component';

describe('ServerSelectComponent', () => {
  let component: ServerSelectComponent;
  let fixture: ComponentFixture<ServerSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
