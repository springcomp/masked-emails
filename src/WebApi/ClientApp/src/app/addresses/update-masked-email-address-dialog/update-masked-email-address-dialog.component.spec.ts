import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateMaskedEmailAddressDialogComponent } from './update-masked-email-address-dialog.component';

describe('UpdateMaskedEmailAddressDialogComponent', () => {
  let component: UpdateMaskedEmailAddressDialogComponent;
  let fixture: ComponentFixture<UpdateMaskedEmailAddressDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateMaskedEmailAddressDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateMaskedEmailAddressDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
