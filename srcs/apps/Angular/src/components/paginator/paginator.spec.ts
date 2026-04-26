import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi, afterEach } from 'vitest';
import { PaginatorComponent } from './paginator';
import { ComponentsModule } from '@components/components.module';

describe('Components.Paginator', () => {
  let component: PaginatorComponent;
  let fixture: ComponentFixture<PaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentsModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.currentPage).toBe(1);
    expect(component.totalPages).toBe(1);
    expect(component.maxVisiblePages).toBe(5);
    expect(component.visiblePages).toEqual([]);
  });

  it('should update visible pages on init', () => {
    // Reset with new values before first detectChanges
    component.currentPage = 1;
    component.totalPages = 10;
    component.maxVisiblePages = 5;
    // Call ngOnChanges to simulate input property changes
    component.ngOnChanges({
      currentPage: { currentValue: 1, previousValue: 0, firstChange: true, isFirstChange: () => true },
      totalPages: { currentValue: 10, previousValue: 0, firstChange: true, isFirstChange: () => true },
      maxVisiblePages: { currentValue: 5, previousValue: 0, firstChange: true, isFirstChange: () => true }
    } as any);

    expect(component.visiblePages).toEqual([1, 2, 3, 4, 5]);
  });

  it('should center current page in visible pages when possible', () => {
    component.currentPage = 5;
    component.totalPages = 10;
    component.maxVisiblePages = 5;
    component.ngOnChanges({
      currentPage: { currentValue: 5, previousValue: 1, firstChange: true, isFirstChange: () => true }
    } as any);

    expect(component.visiblePages).toEqual([3, 4, 5, 6, 7]);
  });

  it('should adjust visible pages near start', () => {
    component.currentPage = 2;
    component.totalPages = 10;
    component.maxVisiblePages = 5;
    component.ngOnChanges({
      currentPage: { currentValue: 2, previousValue: 1, firstChange: true, isFirstChange: () => true }
    } as any);

    expect(component.visiblePages).toEqual([1, 2, 3, 4, 5]);
  });

  it('should adjust visible pages near end', () => {
    component.currentPage = 9;
    component.totalPages = 10;
    component.maxVisiblePages = 5;
    component.ngOnChanges({
      currentPage: { currentValue: 9, previousValue: 1, firstChange: true, isFirstChange: () => true }
    } as any);

    expect(component.visiblePages).toEqual([6, 7, 8, 9, 10]);
  });

  it('should handle fewer total pages than maxVisiblePages', () => {
    component.currentPage = 1;
    component.totalPages = 3;
    component.maxVisiblePages = 5;
    component.ngOnChanges({
      totalPages: { currentValue: 3, previousValue: 1, firstChange: true, isFirstChange: () => true }
    } as any);

    expect(component.visiblePages).toEqual([1, 2, 3]);
  });

  it('should emit pageChange event when going to a valid page', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 1;
    component.totalPages = 10;

    component.goToPage(3);

    expect(emitSpy).toHaveBeenCalledWith(3);
  });

  it('should not emit pageChange when page is already current', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 5;
    component.totalPages = 10;

    component.goToPage(5);

    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('should not emit pageChange when page is less than 1', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 5;
    component.totalPages = 10;

    component.goToPage(0);

    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('should not emit pageChange when page exceeds totalPages', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 5;
    component.totalPages = 10;

    component.goToPage(11);

    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('should go to first page', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 5;
    component.totalPages = 10;

    component.goToFirstPage();

    expect(emitSpy).toHaveBeenCalledWith(1);
  });

  it('should go to last page', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 1;
    component.totalPages = 10;

    component.goToLastPage();

    expect(emitSpy).toHaveBeenCalledWith(10);
  });

  it('should not go to first page if already on first page', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 1;
    component.totalPages = 10;

    component.goToFirstPage();

    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('should not go to last page if already on last page', () => {
    const emitSpy = vi.spyOn(component.pageChange, 'emit');
    component.currentPage = 10;
    component.totalPages = 10;

    component.goToLastPage();

    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('should update visible pages when totalPages changes', () => {
    component.currentPage = 1;
    component.totalPages = 5;
    component.maxVisiblePages = 5;
    component.updateVisiblePages();

    expect(component.visiblePages).toEqual([1, 2, 3, 4, 5]);

    component.totalPages = 3;
    component.updateVisiblePages();

    expect(component.visiblePages).toEqual([1, 2, 3]);
  });

  it('should update visible pages when maxVisiblePages changes', () => {
    component.currentPage = 5;
    component.totalPages = 20;
    component.maxVisiblePages = 5;
    component.updateVisiblePages();

    expect(component.visiblePages).toEqual([3, 4, 5, 6, 7]);

    component.maxVisiblePages = 3;
    component.updateVisiblePages();

    expect(component.visiblePages).toEqual([4, 5, 6]);
  });

  it('should handle single page scenario', () => {
    component.currentPage = 1;
    component.totalPages = 1;
    component.maxVisiblePages = 5;
    component.updateVisiblePages();

    expect(component.visiblePages).toEqual([1]);
  });

  it('should handle large page numbers', () => {
    component.currentPage = 100;
    component.totalPages = 200;
    component.maxVisiblePages = 5;
    component.updateVisiblePages();

    expect(component.visiblePages).toEqual([98, 99, 100, 101, 102]);
  });

  it('should trigger updateVisiblePages on ngOnChanges with currentPage', () => {
    const updateSpy = vi.spyOn(component, 'updateVisiblePages');
    component.ngOnChanges({
      currentPage: { currentValue: 2, previousValue: 1, firstChange: false, isFirstChange: () => false }
    } as any);

    expect(updateSpy).toHaveBeenCalled();
  });

  it('should trigger updateVisiblePages on ngOnChanges with totalPages', () => {
    const updateSpy = vi.spyOn(component, 'updateVisiblePages');
    component.ngOnChanges({
      totalPages: { currentValue: 15, previousValue: 10, firstChange: false, isFirstChange: () => false }
    } as any);

    expect(updateSpy).toHaveBeenCalled();
  });

  it('should trigger updateVisiblePages on ngOnChanges with maxVisiblePages', () => {
    const updateSpy = vi.spyOn(component, 'updateVisiblePages');
    component.ngOnChanges({
      maxVisiblePages: { currentValue: 7, previousValue: 5, firstChange: false, isFirstChange: () => false }
    } as any);

    expect(updateSpy).toHaveBeenCalled();
  });
});
