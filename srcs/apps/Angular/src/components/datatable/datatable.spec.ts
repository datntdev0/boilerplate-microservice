import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DatatableComponent } from './datatable';
import { ComponentsModule } from '@components/components-module';

describe('Components.Datatable', () => {
  let component: DatatableComponent;
  let fixture: ComponentFixture<DatatableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentsModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DatatableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
