import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi, afterEach } from 'vitest';
import { DialogService } from '@components/dialog/dialog.service';
import { PermissionService } from '@shared/services/permission.service';
import { PaginatedResultOfUserListDto, PaginatedResultOfRoleListDto, SrvIdentityClientProxy, UserListDto, RoleListDto } from '@shared/proxies/srv-identity-proxies';
import { Datatable } from '@shared/models/datatable';
import { of, Subject } from 'rxjs';
import { AuthorizationModule } from '../authorization.module';
import { UsersPage } from './users';

describe('Pages.Users', () => {
  let component: UsersPage;
  let fixture: ComponentFixture<UsersPage>;
  let mockSrvIdentityClient: Partial<SrvIdentityClientProxy>;
  let mockDialogService: Partial<DialogService>;
  let mockPermissionService: Partial<PermissionService>;

  beforeEach(async () => {
    mockSrvIdentityClient = {
      users_GetAll: vi.fn().mockReturnValue(of(new PaginatedResultOfUserListDto({
        items: [
          new UserListDto({ id: 1, firstName: 'John', lastName: 'Doe', createdAt: new Date('2024-01-01T00:00:00Z'), updatedAt: new Date('2024-01-01T00:00:00Z') })
        ],
        total: 1,
        offset: 0,
        limit: 10
      }))),
      users_Get: vi.fn(),
      users_Create: vi.fn(),
      users_Update: vi.fn(),
      users_Delete: vi.fn(),
      roles_GetAll: vi.fn().mockReturnValue(of(new PaginatedResultOfRoleListDto({
        items: [],
        total: 0,
        offset: 0,
        limit: 100
      })))
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

    fixture = TestBed.createComponent(UsersPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load users on init', () => {
    expect(mockSrvIdentityClient.users_GetAll).toHaveBeenCalled();
  });

  it('should initialize forms on init', () => {
    expect(component.createForm).toBeDefined();
    expect(component.updateForm).toBeDefined();
  });

  it('should have datatable columns configured', () => {
    expect(component.columns).toBeDefined();
    expect(component.columns.length).toBeGreaterThan(0);
    const firstNameColumn = component.columns.find(col => col.key === 'firstName');
    expect(firstNameColumn).toBeDefined();
    expect(firstNameColumn?.title).toBe('First Name');
  });

  describe('onPageChange', () => {
    it('should call users_GetAll with offset=10 when page=2 and limit=10', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const mockResponse = new PaginatedResultOfUserListDto({ items: [], total: 0, offset: 10, limit: 10 });
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange({ currentPage: 2, pageSize: 10 });
      expect(mockSrvIdentityClient.users_GetAll).toHaveBeenCalledWith(10, 10);
    });

    it('should call users_GetAll with offset=20 when page=3 and limit=10', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const mockResponse = new PaginatedResultOfUserListDto({ items: [], total: 0, offset: 20, limit: 10 });
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange({ currentPage: 3, pageSize: 10 });
      expect(mockSrvIdentityClient.users_GetAll).toHaveBeenCalledWith(20, 10);
    });

    it('should call users_GetAll with offset=0 when page=1 and limit=10', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const mockResponse = new PaginatedResultOfUserListDto({ items: [], total: 0, offset: 0, limit: 10 });
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange({ currentPage: 1, pageSize: 10 });
      expect(mockSrvIdentityClient.users_GetAll).toHaveBeenCalledWith(0, 10);
    });

    it('should update datatableSignal after onPageChange resolves', () => {
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      const newUser = new UserListDto({ id: 2, firstName: 'Jane', lastName: 'Smith' });
      const mockResponse = new PaginatedResultOfUserListDto({ items: [newUser], total: 1, offset: 10, limit: 10 });
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange({ currentPage: 2, pageSize: 10 });
      expect(component.datatableSignal().items).toEqual([newUser]);
    });
  });

  describe('isDataLoadingSignal', () => {
    it('should be true while data is being fetched', () => {
      const subject = new Subject<any>();
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(subject.asObservable());
      component.onPageChange({ currentPage: 1, pageSize: 10 });
      expect(component.isDataLoadingSignal()).toBe(true);
      subject.complete();
    });

    it('should be false after data fetch completes successfully', () => {
      const mockResponse = new PaginatedResultOfUserListDto({ items: [], total: 0, offset: 0, limit: 10 });
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(of(mockResponse));
      component.onPageChange({ currentPage: 1, pageSize: 10 });
      expect(component.isDataLoadingSignal()).toBe(false);
    });

    it('should be false after data fetch fails', () => {
      vi.useFakeTimers();
      const subject = new Subject<any>();
      component.datatableSignal.set(new Datatable({ limit: 10 }));
      (mockSrvIdentityClient.users_GetAll as any).mockReturnValue(subject.asObservable());
      component.onPageChange({ currentPage: 1, pageSize: 10 });
      subject.error(new Error('Network error'));
      expect(component.isDataLoadingSignal()).toBe(false);
      vi.useRealTimers();
    });
  });
});
