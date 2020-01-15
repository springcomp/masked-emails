import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewMaskedEmailAddressDialogComponent } from './new-masked-email-address-dialog.component';

describe('NewMaskedEmailAddressDialogComponent', () => {
  let component: NewMaskedEmailAddressDialogComponent;
  let fixture: ComponentFixture<NewMaskedEmailAddressDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewMaskedEmailAddressDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewMaskedEmailAddressDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
