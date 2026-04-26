import { TestBed } from '@angular/core/testing';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { describe, it, beforeEach, afterEach, expect, vi } from 'vitest';
import { DialogService } from './dialog-service';
import { DialogComponent } from './dialog';

interface DialogConfigWithCallbacks {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  confirmButtonClass?: string;
  cancelButtonClass?: string;
  icon?: string;
  iconColor?: string;
  onConfirm?: () => void;
  onCancel?: () => void;
}

describe('Services.DialogService', () => {
  let service: DialogService;
  let modalService: BsModalService;
  let mockModalRef: BsModalRef;

  beforeEach(() => {
    // Create mock modal reference
    mockModalRef = {
      hide: vi.fn(),
      onHidden: { subscribe: vi.fn(() => ({ unsubscribe: vi.fn() })) }
    } as any;

    // Create mock modal service
    const mockModalService: Partial<BsModalService> = {
      show: vi.fn().mockReturnValue(mockModalRef)
    };

    TestBed.configureTestingModule({
      providers: [
        DialogService,
        { provide: BsModalService, useValue: mockModalService }
      ]
    });

    service = TestBed.inject(DialogService);
    modalService = TestBed.inject(BsModalService);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should create', () => {
    expect(service).toBeTruthy();
  });

  describe('confirmDelete', () => {
    it('should display dialog with delete configuration', async () => {
      const message = 'Are you sure you want to delete?';
      const title = 'Delete Item';

      const resultPromise = service.confirmDelete(message, title).toPromise();

      // Access the initialState that was passed to modalService.show
      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe(title);
      expect(initialState.message).toBe(message);
      expect(initialState.confirmText).toBe('Delete');
      expect(initialState.cancelText).toBe('Cancel');
      expect(initialState.confirmButtonClass).toBe('btn-danger');
      expect(initialState.cancelButtonClass).toBe('btn-secondary');
      expect(initialState.icon).toBe('bi-trash-fill');
      expect(initialState.iconColor).toBe('text-danger');

      // Trigger the onConfirm callback
      initialState.onConfirm?.();
      const result = await resultPromise;
      expect(result).toBe(true);
      expect(mockModalRef.hide).toHaveBeenCalled();
    });

    it('should use default title when not provided', async () => {
      const resultPromise = service.confirmDelete('Delete this item').toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe('Confirm Deletion');
      initialState.onConfirm?.();
      await resultPromise;
    });

    it('should emit false when cancel is clicked', async () => {
      const resultPromise = service.confirmDelete('Delete this item').toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      // Trigger the onCancel callback
      initialState.onCancel?.();
      const result = await resultPromise;
      expect(result).toBe(false);
      expect(mockModalRef.hide).toHaveBeenCalled();
    });
  });

  describe('info', () => {
    it('should display info dialog with default title', async () => {
      const message = 'This is an informational message';

      const resultPromise = service.info(message).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe('Information');
      expect(initialState.message).toBe(message);
      expect(initialState.confirmText).toBe('OK');
      expect(initialState.confirmButtonClass).toBe('btn-primary');
      expect(initialState.icon).toBe('bi-info-circle-fill');
      expect(initialState.iconColor).toBe('text-primary');

      initialState.onConfirm?.();
      const result = await resultPromise;
      expect(result).toBe(true);
    });

    it('should use custom title when provided', async () => {
      const customTitle = 'Custom Info';

      const resultPromise = service.info('Message', customTitle).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe(customTitle);
      initialState.onConfirm?.();
      await resultPromise;
    });
  });

  describe('success', () => {
    it('should display success dialog with correct configuration', async () => {
      const message = 'Operation completed successfully';

      const resultPromise = service.success(message).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe('Success');
      expect(initialState.message).toBe(message);
      expect(initialState.confirmText).toBe('OK');
      expect(initialState.confirmButtonClass).toBe('btn-success');
      expect(initialState.icon).toBe('bi-check-circle-fill');
      expect(initialState.iconColor).toBe('text-success');

      initialState.onConfirm?.();
      const result = await resultPromise;
      expect(result).toBe(true);
    });

    it('should use custom title for success dialog', async () => {
      const customTitle = 'Changes Saved';

      const resultPromise = service.success('Your changes have been saved', customTitle).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe(customTitle);
      initialState.onConfirm?.();
      await resultPromise;
    });
  });

  describe('warning', () => {
    it('should display warning dialog with correct configuration', async () => {
      const message = 'This action may have consequences';

      const resultPromise = service.warning(message).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe('Warning');
      expect(initialState.message).toBe(message);
      expect(initialState.confirmText).toBe('OK');
      expect(initialState.confirmButtonClass).toBe('btn-warning');
      expect(initialState.icon).toBe('bi-exclamation-triangle-fill');
      expect(initialState.iconColor).toBe('text-warning');

      initialState.onConfirm?.();
      const result = await resultPromise;
      expect(result).toBe(true);
    });
  });

  describe('error', () => {
    it('should display error dialog with correct configuration', async () => {
      const message = 'An error occurred';

      const resultPromise = service.error(message).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe('Error');
      expect(initialState.message).toBe(message);
      expect(initialState.confirmText).toBe('OK');
      expect(initialState.confirmButtonClass).toBe('btn-danger');
      expect(initialState.icon).toBe('bi-x-circle-fill');
      expect(initialState.iconColor).toBe('text-danger');

      initialState.onConfirm?.();
      const result = await resultPromise;
      expect(result).toBe(true);
    });

    it('should use custom title for error dialog', async () => {
      const customTitle = 'Validation Error';

      const resultPromise = service.error('Please check your input', customTitle).toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      expect(initialState.title).toBe(customTitle);
      initialState.onConfirm?.();
      await resultPromise;
    });
  });

  describe('modal configuration', () => {
    it('should configure modal with correct settings', async () => {
      const resultPromise = service.info('Test message').toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const component = callArgs[0];
      const options = callArgs[1];

      expect(component).toBe(DialogComponent);
      expect(options.class).toBe('modal-dialog-centered');
      expect(options.backdrop).toBe('static');
      expect(options.keyboard).toBe(true);

      const initialState: DialogConfigWithCallbacks = options.initialState;
      initialState.onConfirm?.();
      await resultPromise;
    });
  });

  describe('modal lifecycle', () => {
    it('should hide modal on confirm', async () => {
      const resultPromise = service.confirmDelete('Delete?').toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      initialState.onConfirm?.();
      await resultPromise;
      expect(mockModalRef.hide).toHaveBeenCalledTimes(1);
    });

    it('should hide modal on cancel', async () => {
      const resultPromise = service.confirmDelete('Delete?').toPromise();

      const callArgs = (modalService.show as any).mock.calls[0];
      const initialState: DialogConfigWithCallbacks = callArgs[1].initialState;

      initialState.onCancel?.();
      await resultPromise;
      expect(mockModalRef.hide).toHaveBeenCalledTimes(1);
    });
  });

  describe('multiple dialogs', () => {
    it('should handle sequential dialog calls', async () => {
      const results: boolean[] = [];

      service.info('First dialog').subscribe((result) => {
        results.push(result);
      });

      let firstArgs = (modalService.show as any).mock.calls[0];
      let firstInitialState: DialogConfigWithCallbacks = firstArgs[1].initialState;
      firstInitialState.onConfirm?.();

      // Wait a bit for first observable to complete
      await new Promise(resolve => setTimeout(resolve, 10));

      vi.clearAllMocks();
      mockModalRef = {
        hide: vi.fn(),
        onHidden: { subscribe: vi.fn(() => ({ unsubscribe: vi.fn() })) }
      } as any;
      (modalService.show as any).mockReturnValue(mockModalRef);

      service.warning('Second dialog').subscribe((result) => {
        results.push(result);
      });

      let secondArgs = (modalService.show as any).mock.calls[0];
      let secondInitialState: DialogConfigWithCallbacks = secondArgs[1].initialState;
      secondInitialState.onConfirm?.();

      // Wait a bit for second observable to complete
      await new Promise(resolve => setTimeout(resolve, 10));

      expect(results).toEqual([true, true]);
    });
  });
});
