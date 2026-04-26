import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi, afterEach } from 'vitest';
import { ToastComponent } from './toast';
import { ToastService } from './toast-service';

describe('Components.Toast', () => {
  let component: ToastComponent;
  let fixture: ComponentFixture<ToastComponent>;
  let toastService: ToastService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ToastComponent],
      providers: [ToastService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ToastComponent);
    component = fixture.componentInstance;
    toastService = TestBed.inject(ToastService);
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should subscribe to toastService on init', () => {
    expect(component.toastsSignal()).toEqual([]);
  });

  it('should update toastsSignal when toastService emits', async () => {
    toastService.show({ type: 'success', title: 'Test 1' });
    toastService.show({ type: 'danger', title: 'Test 2' });

    await new Promise(resolve => setTimeout(resolve, 50));
    expect(component.toastsSignal().length).toBe(2);
  });

  it('should remove toast by id', async () => {
    toastService.show({ type: 'success', title: 'Test Toast' });

    const toastId = component.toastsSignal()[0]?.id;
    component.remove(toastId || '');

    await new Promise(resolve => setTimeout(resolve, 50));
    expect(component.toastsSignal().length).toBe(0);
  });

  it('should get correct toast class for success type', () => {
    const toastClass = component.getToastClass('success');
    expect(toastClass).toBe('toast toast-success');
  });

  it('should get correct toast class for danger type', () => {
    const toastClass = component.getToastClass('danger');
    expect(toastClass).toBe('toast toast-danger');
  });

  it('should get correct toast class for warning type', () => {
    const toastClass = component.getToastClass('warning');
    expect(toastClass).toBe('toast toast-warning');
  });

  it('should get correct toast class for info type', () => {
    const toastClass = component.getToastClass('info');
    expect(toastClass).toBe('toast toast-info');
  });

  it('should unsubscribe on destroy', () => {
    const subscription = component['subscription'];
    expect(subscription).toBeDefined();

    component.ngOnDestroy();

    if (subscription) {
      expect(subscription.closed).toBeTruthy();
    }
  });

  it('should handle destroy without subscription', () => {
    component['subscription'] = undefined;
    expect(() => component.ngOnDestroy()).not.toThrow();
  });
});

describe('Services.ToastService', () => {
  let service: ToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ToastService]
    });
    service = TestBed.inject(ToastService);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should emit toast when show is called', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.show({ type: 'success', title: 'Test' });

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts).toBeTruthy();
    expect(emittedToasts[0].type).toBe('success');
    expect(emittedToasts[0].title).toBe('Test');
  });

  it('should show success toast', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.success('Success Message');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].type).toBe('success');
    expect(emittedToasts[0].title).toBe('Success Message');
  });

  it('should show error toast with danger type', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.error('Error Message');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].type).toBe('danger');
    expect(emittedToasts[0].title).toBe('Error Message');
  });

  it('should show warning toast', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.warning('Warning Message');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].type).toBe('warning');
    expect(emittedToasts[0].title).toBe('Warning Message');
  });

  it('should show info toast', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.info('Info Message');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].type).toBe('info');
    expect(emittedToasts[0].title).toBe('Info Message');
  });

  it('should add message to toast', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.show({ type: 'success', title: 'Test', message: 'Additional info' });

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].message).toBe('Additional info');
  });

  it('should set custom duration for toast', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.show({ type: 'success', title: 'Test', duration: 3000 });

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].duration).toBe(3000);
  });

  it('should auto-remove toast after default duration', async () => {
    vi.useFakeTimers();

    service.show({ type: 'success', title: 'Test' });
    expect(service['toasts'].length).toBe(1);

    vi.advanceTimersByTime(5100);

    expect(service['toasts'].length).toBe(0);
    vi.useRealTimers();
  });

  it('should not auto-remove toast with duration 0', async () => {
    vi.useFakeTimers();

    service.show({ type: 'success', title: 'Test', duration: 0 });
    expect(service['toasts'].length).toBe(1);

    vi.advanceTimersByTime(10000);

    expect(service['toasts'].length).toBe(1);
    vi.useRealTimers();
  });

  it('should auto-remove toast after custom duration', async () => {
    vi.useFakeTimers();
    const customDuration = 2000;

    service.show({ type: 'success', title: 'Test', duration: customDuration });
    expect(service['toasts'].length).toBe(1);

    vi.advanceTimersByTime(customDuration + 100);

    expect(service['toasts'].length).toBe(0);
    vi.useRealTimers();
  });

  it('should remove toast by id', async () => {
    service.show({ type: 'success', title: 'Test' });

    await new Promise(resolve => setTimeout(resolve, 10));
    const toastId = service['toasts'][0].id;

    service.remove(toastId);

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(service['toasts'].length).toBe(0);
  });

  it('should clear all toasts', async () => {
    service.show({ type: 'success', title: 'Test 1' });
    service.show({ type: 'danger', title: 'Test 2' });

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(service['toasts'].length).toBe(2);

    service.clear();

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(service['toasts'].length).toBe(0);
  });

  it('should generate unique ids for toasts', async () => {
    service.show({ type: 'success', title: 'Test 1' });
    service.show({ type: 'danger', title: 'Test 2' });

    const toast1 = service['toasts'][0];
    const toast2 = service['toasts'][1];

    expect(toast1.id).not.toBe(toast2.id);
    expect(toast1.id).toMatch(/^toast-\d+-\w+$/);
    expect(toast2.id).toMatch(/^toast-\d+-\w+$/);
  });

  it('should set createdAt timestamp', async () => {
    const beforeTime = new Date().toISOString();
    
    service.show({ type: 'success', title: 'Test' });
    
    const afterTime = new Date().toISOString();

    await new Promise(resolve => setTimeout(resolve, 10));
    const toastCreatedAt = service['toasts'][0].createdAt || '';
    
    expect(toastCreatedAt >= beforeTime).toBeTruthy();
    expect(toastCreatedAt <= afterTime).toBeTruthy();
  });

  it('should handle multiple toasts', async () => {
    service.show({ type: 'success', title: 'Test 1' });
    service.show({ type: 'danger', title: 'Test 2' });
    service.show({ type: 'warning', title: 'Test 3' });

    await new Promise(resolve => setTimeout(resolve, 50));

    expect(service['toasts'].length).toBe(3);
    expect(service['toasts'][0].title).toBe('Test 1');
    expect(service['toasts'][1].title).toBe('Test 2');
    expect(service['toasts'][2].title).toBe('Test 3');
  });

  it('should preserve message in error method', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.error('Error Title', 'Error details');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].message).toBe('Error details');
  });

  it('should preserve message in warning method', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.warning('Warning Title', 'Warning details');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].message).toBe('Warning details');
  });

  it('should preserve message in info method', async () => {
    let emittedToasts: any = null;

    service.toasts$.subscribe((toasts) => {
      emittedToasts = toasts;
    });

    service.info('Info Title', 'Info details');

    await new Promise(resolve => setTimeout(resolve, 10));
    expect(emittedToasts[0].message).toBe('Info details');
  });
});
