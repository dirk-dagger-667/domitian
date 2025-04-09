import { RegisterConfirmationComponent } from './register-confirmation.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';

describe('RegisterConfirmationComponent', () => {
  let component: RegisterConfirmationComponent;
  let fixture: ComponentFixture<RegisterConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterConfirmationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
