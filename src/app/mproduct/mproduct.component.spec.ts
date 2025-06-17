import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MproductComponent } from './mproduct.component';

describe('MproductComponent', () => {
  let component: MproductComponent;
  let fixture: ComponentFixture<MproductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MproductComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MproductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
