import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegLogHeaderWidgetComponent } from './reg-log-header-widget.component';

describe('RegLogHeaderWidgetComponent', () => {
  let component: RegLogHeaderWidgetComponent;
  let fixture: ComponentFixture<RegLogHeaderWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegLogHeaderWidgetComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegLogHeaderWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
