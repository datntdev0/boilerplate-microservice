import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi, afterEach } from 'vitest';
import { DialogService } from '@components/dialog/dialog.service';
import { PermissionService } from '@shared/services/permission.service';
import { PaginatedResultOfRoleListDto, RoleListDto, SrvIdentityClientProxy } from '@shared/proxies/srv-identity-proxies';
import { Datatable } from '@shared/models/datatable';
import { of, Subject } from 'rxjs';
import { AuthorizationModule } from '../authorization.module';
import { RolesPage } from './roles';

describe('Pages.Roles', () => {
  let component: RolesPage;
  let fixture: ComponentFixture<RolesPage>;
  let mockSrvIdentityClient: Partial<SrvIdentityClientProxy>;
  let mockDialogService: Partial<DialogService>;
  let mockPermissionService: Partial<PermissionService>;

  beforeEach(async () => {
    mockSrvIdentityClient = {
      roles_GetAll: vi.fn().mockReturnValue(of(new PaginatedResultOfRoleListDto({
        items: [
          new RoleListDto({ id: 1, name: 'Admin', description: 'Admin role', createdAt: new Date('2024-01-01T00:00:00Z'), updatedAt: new Date('2024-01-01T00:00:00Z') })
        ],
        total: 1,
        offset: 0,
        limit: 10
      }))),
      roles_Get: vi.fn(),
      roles_Create: vi.fn(),
      roles_Update: vi.fn(),
      roles_Delete: vi.fn()
    };

    mockDialogService = {
      confirmDelete: vi.fn().mockReturnValue(of(true))
    };

    mockPermissionService = {
      buildTree: vi.fn().mockReturnValue([]),
      extractPermissions: vi.fn().mockReturnValue([])
    };

    await TestBed.configureTestingModule({
      imports: [AuthorizationModule],
      providers: [
        { provide: SrvIdentityClientProxy, useValue: mockSrvIdentityClient },
        { provide: DialogService, useValue: mockDialogService },
        { provide: PermissionService, useValue: mockPermissionService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RolesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load roles on init', () => {
    expect(mockSrvIdentityClient.roles_GetAll).toHaveBeenCalled();
  });

  it('should initialize forms on init', () => {
    expect(component.createForm).toBeDefined();
    expect(component.updateForm).toBeDefined();
  });

  it('should have datatable columns configured', () => {
    expect(component.columns).toBeDefined();
    expect(component.columns.length).toBeGreaterThan(0);
    const nameColumn = component.columns.find(col => col.key === 'name');
    expect(nameColumn).toBeDefined();
    expect(nameColumn?.title).toBe('Role Name');
  });

  describe('onPageChange', () => {
    it('should call roles_GetAll with offset=10 when page=2 and limit=10', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const mockResponse = new PaginatedResultOfRoleListDto({ items: [], total: 0, offset: 10, limit: 10 });
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange(2);
      expect(mockSrvIdentityClient.roles_GetAll).toHaveBeenCalledWith(10, 10);
    });

    it('should call roles_GetAll with offset=20 when page=3 and limit=10', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const mockResponse = new PaginatedResultOfRoleListDto({ items: [], total: 0, offset: 20, limit: 10 });
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange(3);
      expect(mockSrvIdentityClient.roles_GetAll).toHaveBeenCalledWith(20, 10);
    });

    it('should call roles_GetAll with offset=0 when page=1 and limit=10', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const mockResponse = new PaginatedResultOfRoleListDto({ items: [], total: 0, offset: 0, limit: 10 });
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange(1);
      expect(mockSrvIdentityClient.roles_GetAll).toHaveBeenCalledWith(0, 10);
    });

    it('should update datatableSignal after onPageChange resolves', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const newRole = new RoleListDto({ id: 2, name: 'Editor', description: 'Editor role' });
      const mockResponse = new PaginatedResultOfRoleListDto({ items: [newRole], total: 1, offset: 10, limit: 10 });
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange(2);
      expect(component.datatableSignal().items).toEqual([newRole]);
    });
  });

  describe('isDataLoadingSignal', () => {
    it('should be true while data is being fetched', () => {
      const subject = new Subject<any>();
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(subject.asObservable());
      component.onPageChange(1);
      expect(component.isDataLoadingSignal()).toBe(true);
      subject.complete();
    });

    it('should be false after data fetch completes successfully', () => {
      const mockResponse = new PaginatedResultOfRoleListDto({ items: [], total: 0, offset: 0, limit: 10 });
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange(1);
      expect(component.isDataLoadingSignal()).toBe(false);
    });

    it('should be false after data fetch fails', () => {
      vi.useFakeTimers();
      const subject = new Subject<any>();
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      (mockSrvIdentityClient.roles_GetAll as any).mockReturnValue(subject.asObservable());
      component.onPageChange(1);
      subject.error(new Error('Network error'));
      expect(component.isDataLoadingSignal()).toBe(false);
      vi.useRealTimers();
    });
  });
});
