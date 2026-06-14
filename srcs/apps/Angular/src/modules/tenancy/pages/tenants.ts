import { AfterViewInit, Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DatatableComponent } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog.service';
import { Datatable } from '@shared/models/datatable';
import { SrvAdminClientProxy, TenantCreateDto, TenantListDto, TenantUpdateDto } from '@shared/proxies/srv-admin-proxies';
import {
  SrvIdentityClientProxy,
  TenantUserListDto,
  TenantUsersInviteDto,
  TenantUsersPatchDto,
} from '@shared/proxies/srv-identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './tenants.html',
})
export class TenantsPage implements OnInit, AfterViewInit {
  private readonly clientAdminSrv = inject(SrvAdminClientProxy);
  private readonly clientIdentitySrv = inject(SrvIdentityClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);

  @ViewChild('assignedUsersDatatableRef') assignedUsersDatatableRef?: DatatableComponent;

  public datatableSignal = signal(new Datatable<TenantListDto>());
  public isLoadingSignal = signal(false);
  public isDataLoadingSignal = signal(false);

  // User assignment panel state
  public assignedUsersDatatable = signal(new Datatable<TenantUserListDto>());
  public isUsersLoadingSignal = signal(false);
  public isInviteLoadingSignal = signal(false);
  public hasSelectedUsers = signal(false);
  public inviteWarning = signal<string | null>(null);
  public inviteSuccess = signal<string | null>(null);

  editingTenant: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  emailTagsControl = new FormControl<string[]>([]);

  columns: DatatableColumn[] = [
    {
      key: 'name',
      title: 'Tenant Name',
      datatype: 'string'
    },
    {
      key: 'organization',
      title: 'Organization',
      datatype: 'string'
    },
    {
      key: 'createdAt',
      title: 'Created',
      datatype: 'date'
    },
    {
      key: 'updatedAt',
      title: 'Updated',
      datatype: 'date'
    }
  ];

  assignedUsersColumns: DatatableColumn[] = [
    {
      key: 'email',
      title: 'Email Address',
      datatype: 'string'
    },
    {
      key: 'fullName',
      title: 'Full Name',
      datatype: 'string'
    },
    {
      key: 'assignedDate',
      title: 'Assigned Date',
      datatype: 'date'
    }
  ];

  ngOnInit(): void {
    this.createForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      organization: [''],
    });
    this.updateForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      organization: [''],
    });

    this.fetchTenants(0, 10);
  }

  private fetchTenants(offset: number, limit: number): void {
    this.isDataLoadingSignal.set(true);
    this.clientAdminSrv.tenants_GetAll(offset, limit).subscribe({
      next: tenants => {
        this.datatableSignal.set(new Datatable<TenantListDto>(tenants));
        this.isDataLoadingSignal.set(false);
      },
      error: (err) => {
        this.isDataLoadingSignal.set(false);
        throw err;
      }
    });
  }

  private fetchAssignedUsers(tenantId: number, offset: number, limit: number): void {
    this.isUsersLoadingSignal.set(true);
    this.clientIdentitySrv.tenantUsers_GetAll(tenantId, offset, limit).subscribe({
      next: result => {
        this.assignedUsersDatatable.set(new Datatable<TenantUserListDto>(result));
        this.hasSelectedUsers.set(false);
        this.isUsersLoadingSignal.set(false);
      },
      error: (err) => {
        this.isUsersLoadingSignal.set(false);
        throw err;
      }
    });
  }

  ngAfterViewInit(): void {
    // Empty - no longer needed for tooltip initialization
  }

  protected onCreate(modal: ModalDirective): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isLoadingSignal.set(true);
    const data = new TenantCreateDto({
      name: this.createForm.value.name,
      organization: this.createForm.value.organization,
    })

    this.clientAdminSrv.tenants_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isLoadingSignal.set(false);
          this.fetchTenants(0, 10);
          modal.hide();
        },
        error: (err) => {
          this.isLoadingSignal.set(false);
          throw err;
        }
      });
  }

  protected onUpdate(modal: ModalDirective): void {
    if (this.updateForm.invalid) {
      this.updateForm.markAllAsTouched();
      return;
    }

    this.isLoadingSignal.set(true);
    const data = new TenantUpdateDto({
      id: this.editingTenant.id,
      name: this.updateForm.value.name,
      organization: this.updateForm.value.organization,
    });

    this.clientAdminSrv.tenants_Update(this.editingTenant.id, data)
      .subscribe({
        next: () => {
          this.updateForm.reset();
          this.isLoadingSignal.set(false);
          this.fetchTenants(0, 10);
          modal.hide();
        },
        error: (err) => {
          this.isLoadingSignal.set(false);
          throw err;
        }
      });
  }

  protected onInvite(): void {
    const emails: string[] = this.emailTagsControl.value ?? [];
    if (emails.length === 0) return;

    this.isInviteLoadingSignal.set(true);
    this.inviteWarning.set(null);
    this.inviteSuccess.set(null);

    const body = new TenantUsersInviteDto({ emails });
    this.clientIdentitySrv.tenantUsers_CreateInvite(this.editingTenant.id, body).subscribe({
      next: result => {
        this.emailTagsControl.setValue([]);
        this.isInviteLoadingSignal.set(false);
        this.fetchAssignedUsers(this.editingTenant.id, 0, 10);

        if (result.unrecognizedEmails && result.unrecognizedEmails.length > 0) {
          this.inviteWarning.set(
            `The following email address(es) were not found: ${result.unrecognizedEmails.join(', ')}`
          );
        } else {
          this.inviteSuccess.set('Users invited successfully.');
        }
      },
      error: (err) => {
        this.isInviteLoadingSignal.set(false);
        throw err;
      }
    });
  }

  protected onRemoveSelected(): void {
    const selectedItems = this.assignedUsersDatatableRef?.selectedItems;
    if (!selectedItems || selectedItems.size === 0) return;

    const ids: number[] = Array.from(selectedItems).map((item: any) => item.userId as number);
    const body = new TenantUsersPatchDto({ delete: ids });

    this.clientIdentitySrv.tenantUsers_Patch(this.editingTenant.id, body).subscribe({
      next: () => {
        if (this.assignedUsersDatatableRef) {
          this.assignedUsersDatatableRef.selectedItems.clear();
          this.assignedUsersDatatableRef.allSelected = false;
        }
        this.hasSelectedUsers.set(false);
        this.inviteWarning.set(null);
        this.inviteSuccess.set(null);
        this.fetchAssignedUsers(this.editingTenant.id, 0, 10);
      },
      error: (err) => {
        throw err;
      }
    });
  }

  protected onAssignedUsersCheckboxChange(count: number): void {
    this.hasSelectedUsers.set(count > 0);
  }

  protected onAssignedUsersPageChange(event: { currentPage: number; pageSize: number }): void {
    const offset = (event.currentPage - 1) * event.pageSize;
    this.fetchAssignedUsers(this.editingTenant.id, offset, event.pageSize);
  }

  onPageChange(event: { currentPage: number; pageSize: number }): void {
    const offset = (event.currentPage - 1) * event.pageSize;
    this.fetchTenants(offset, event.pageSize);
  }

  protected onEdit(item: any, modal: ModalDirective): void {
    this.editingTenant = item;
    this.updateForm.patchValue({ name: item.name, organization: item.organization });
    this.inviteWarning.set(null);
    this.inviteSuccess.set(null);
    this.hasSelectedUsers.set(false);
    this.emailTagsControl.setValue([]);
    this.fetchAssignedUsers(item.id, 0, 10);
    modal.show();
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete tenant "${item.name}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientAdminSrv.tenants_Delete(item.id)
          .subscribe({ next: () => this.fetchTenants(0, 10) });
      });
  }
}
