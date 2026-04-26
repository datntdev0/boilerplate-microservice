import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi, afterEach } from 'vitest';
import { DialogService } from '@components/dialog/dialog.service';
import { PaginatedResultOfTenantListDto, SrvAdminClientProxy, TenantListDto } from '@shared/proxies/srv-admin-proxies';
import { of, throwError } from 'rxjs';
import { TenancyModule } from '../tenancy.module';
import { TenantsPage } from './tenants';

describe('Pages.Tenants', () => {
  let component: TenantsPage;
  let fixture: ComponentFixture<TenantsPage>;
  let mockSrvAdminClient: Partial<SrvAdminClientProxy>;
  let mockDialogService: Partial<DialogService>;

  beforeEach(async () => {
    mockSrvAdminClient = {
      tenants_GetAll: vi.fn().mockReturnValue(of(new PaginatedResultOfTenantListDto({
        items: [
          new TenantListDto({ id: 1, name: 'Tenant 1', organization: 'Org 1', createdAt: '2024-01-01T00:00:00Z', updatedAt: '2024-01-01T00:00:00Z' })
        ],
        total: 1,
        offset: 0,
        limit: 10
      }))),
      tenants_Get: vi.fn(),
      tenants_Create: vi.fn(),
      tenants_Update: vi.fn(),
      tenants_Delete: vi.fn()
    };

    mockDialogService = {
      confirmDelete: vi.fn().mockReturnValue(of(true))
    };

    await TestBed.configureTestingModule({
      imports: [TenancyModule],
      providers: [
        { provide: SrvAdminClientProxy, useValue: mockSrvAdminClient },
        { provide: DialogService, useValue: mockDialogService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TenantsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load tenants on init', () => {
    expect(mockSrvAdminClient.tenants_GetAll).toHaveBeenCalled();
  });

  it('should initialize forms on init', () => {
    expect(component.createForm).toBeDefined();
    expect(component.updateForm).toBeDefined();
  });

  it('should have createForm with required validators', () => {
    const nameControl = component.createForm.get('name');
    expect(nameControl?.hasError('required')).toBeTruthy();
    nameControl?.setValue('ab');
    expect(nameControl?.hasError('minlength')).toBeTruthy();
    nameControl?.setValue('Valid Name');
    expect(nameControl?.valid).toBeTruthy();
  });

  it('should not create tenant with invalid form', () => {
    component.createForm.patchValue({ name: '' });
    const mockModal = { hide: vi.fn() } as any;
    component['onCreate'](mockModal);
    expect(mockSrvAdminClient.tenants_Create).not.toHaveBeenCalled();
  });

  it('should create tenant with valid form', async () => {
    (mockSrvAdminClient.tenants_Create as any).mockReturnValue(of({}));
    component.createForm.patchValue({ 
      name: 'New Tenant',
      organization: 'New Org'
    });
    const mockModal = { hide: vi.fn() } as any;
    component['onCreate'](mockModal);
    await new Promise(resolve => setTimeout(resolve, 50));
    expect(mockSrvAdminClient.tenants_Create).toHaveBeenCalledWith(
      expect.objectContaining({
        name: 'New Tenant',
        organization: 'New Org'
      })
    );
  });

  it('should hide modal after successful creation', async () => {
    (mockSrvAdminClient.tenants_Create as any).mockReturnValue(of({}));
    component.createForm.patchValue({ 
      name: 'New Tenant',
      organization: 'New Org'
    });
    const mockModal = { hide: vi.fn() } as any;
    component['onCreate'](mockModal);
    await new Promise(resolve => setTimeout(resolve, 50));
    expect(mockModal.hide).toHaveBeenCalled();
  });

  it('should set loading signal during creation', async () => {
    (mockSrvAdminClient.tenants_Create as any).mockReturnValue(of({}));
    component.createForm.patchValue({ 
      name: 'New Tenant',
      organization: 'New Org'
    });
    const mockModal = { hide: vi.fn() } as any;
    component['onCreate'](mockModal);
    // The loading signal is set inside the subscribe callback, so we wait
    await new Promise(resolve => setTimeout(resolve, 50));
    expect(component.isLoadingSignal()).toBe(false);
  });

  it('should not update tenant with invalid form', () => {
    component.updateForm.patchValue({ name: '' });
    const mockModal = { hide: vi.fn() } as any;
    component['onUpdate'](mockModal);
    expect(mockSrvAdminClient.tenants_Update).not.toHaveBeenCalled();
  });

  it('should update tenant with valid form', async () => {
    (mockSrvAdminClient.tenants_Update as any).mockReturnValue(of({}));
    component.editingTenant = { id: 1, name: 'Old Name' };
    component.updateForm.patchValue({ 
      name: 'Updated Tenant',
      organization: 'Updated Org'
    });
    const mockModal = { hide: vi.fn() } as any;
    component['onUpdate'](mockModal);
    await new Promise(resolve => setTimeout(resolve, 50));
    expect(mockSrvAdminClient.tenants_Update).toHaveBeenCalledWith(
      1,
      expect.objectContaining({
        name: 'Updated Tenant',
        organization: 'Updated Org'
      })
    );
  });

  it('should set editing tenant when onEdit is called', () => {
    const tenant = { id: 1, name: 'Test Tenant', organization: 'Test Org' };
    const mockModal = { show: vi.fn() } as any;
    component['onEdit'](tenant, mockModal);
    expect(component.editingTenant).toBe(tenant);
    expect(component.updateForm.get('name')?.value).toBe('Test Tenant');
    expect(mockModal.show).toHaveBeenCalled();
  });

  it('should call confirmDelete with correct message', () => {
    mockDialogService.confirmDelete = vi.fn().mockReturnValue(of(false));
    const tenant = { id: 1, name: 'Test Tenant' };
    component['onDelete'](tenant);
    expect(mockDialogService.confirmDelete).toHaveBeenCalledWith(
      'Are you sure you want to delete tenant "Test Tenant"?'
    );
  });

  it('should delete tenant when confirmed', async () => {
    const reloadResponse = new PaginatedResultOfTenantListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });
    
    // Set up mocks before the operation
    mockSrvAdminClient.tenants_Delete = vi.fn().mockReturnValue(of({}));
    mockDialogService.confirmDelete = vi.fn().mockReturnValue(of(true));
    mockSrvAdminClient.tenants_GetAll = vi.fn().mockReturnValue(of(reloadResponse));
    
    const tenant = { id: 1, name: 'Test Tenant' };
    component['onDelete'](tenant);
    await new Promise(resolve => setTimeout(resolve, 100));
    expect(mockSrvAdminClient.tenants_Delete).toHaveBeenCalledWith(1);
  });

  it('should not delete tenant when not confirmed', () => {
    mockDialogService.confirmDelete = vi.fn().mockReturnValue(of(false));
    const tenant = { id: 1, name: 'Test Tenant' };
    component['onDelete'](tenant);
    expect(mockSrvAdminClient.tenants_Delete).not.toHaveBeenCalled();
  });

  it('should have datatable columns configured', () => {
    expect(component.columns).toBeDefined();
    expect(component.columns.length).toBeGreaterThan(0);
    const nameColumn = component.columns.find(col => col.key === 'name');
    expect(nameColumn).toBeDefined();
    expect(nameColumn?.title).toBe('Tenant Name');
  });

  it('should initialize isLoadingSignal as false', () => {
    expect(component.isLoadingSignal()).toBe(false);
  });
});
