import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteLinkComponent } from './delete-link.component';

describe('DeleteLinkComponent', () => {
  let component: DeleteLinkComponent;
  let fixture: ComponentFixture<DeleteLinkComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeleteLinkComponent]
    });
    fixture = TestBed.createComponent(DeleteLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
