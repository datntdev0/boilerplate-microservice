import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi } from 'vitest';
import { DatatableComponent } from './datatable';
import { ComponentsModule } from '@components/components.module';

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
    component.data = [{ id: 1 }, { id: 2 }, { id: 3 }];
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('toggleSelectItem emits selectionChange with count 1 after selecting one item', () => {
    const emitted: number[] = [];
    component.selectionChange.subscribe((n: number) => emitted.push(n));

    component.toggleSelectItem(component.data[0]);

    expect(emitted).toEqual([1]);
    expect(component.selectedItems.size).toBe(1);
  });

  it('toggleSelectItem emits selectionChange with count 0 after deselecting the only selected item', () => {
    component.toggleSelectItem(component.data[0]);
    const emitted: number[] = [];
    component.selectionChange.subscribe((n: number) => emitted.push(n));

    component.toggleSelectItem(component.data[0]);

    expect(emitted).toEqual([0]);
    expect(component.selectedItems.size).toBe(0);
  });

  it('toggleSelectAll emits selectionChange with full count when selecting all', () => {
    component.allSelected = true;
    const emitted: number[] = [];
    component.selectionChange.subscribe((n: number) => emitted.push(n));

    component.toggleSelectAll();

    expect(emitted).toEqual([3]);
    expect(component.selectedItems.size).toBe(3);
  });

  it('toggleSelectAll emits selectionChange with 0 when deselecting all', () => {
    component.allSelected = true;
    component.toggleSelectAll();
    component.allSelected = false;
    const emitted: number[] = [];
    component.selectionChange.subscribe((n: number) => emitted.push(n));

    component.toggleSelectAll();

    expect(emitted).toEqual([0]);
    expect(component.selectedItems.size).toBe(0);
  });
});
