import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog.service';
import { Datatable } from '@shared/models/datatable';
import { PermissionNode, PermissionService } from '@shared/services/permission.service';
import { RoleListDto, SrvIdentityClientProxy, UserCreateDto, UserListDto, UserUpdateDto } from '@shared/proxies/srv-identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './users.html',
})
export class UsersPage implements OnInit {
  private readonly clientIdentitySrv = inject(SrvIdentityClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);
  private readonly permissionSrv = inject(PermissionService);

  public datatableSignal = signal(new Datatable<UserListDto>());
  public allRolesSignal = signal(new Datatable<RoleListDto>());
  public isLoadingSignal = signal(false);
  public isDataLoadingSignal = signal(false);

  editingUser: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  createPermTree: PermissionNode[] = [];
  updatePermTree: PermissionNode[] = [];
  selectedRoleIds: number[] = [];

  columns: DatatableColumn[] = [
    {
      key: 'emailAddress',
      title: 'Email Address',
      datatype: 'string'
    },
    {
      key: 'lastName',
      title: 'Last Name',
      datatype: 'string'
    },
    {
      key: 'firstName',
      title: 'First Name',
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

  ngOnInit(): void {
    this.createForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
    });
    this.updateForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
    });

    this.fetchUsers(0, 10);

    this.clientIdentitySrv.roles_GetAll(0, 100).subscribe(
      roles => {
        this.allRolesSignal.set(new Datatable<RoleListDto>(roles));
      }
    );
  }

  private fetchUsers(offset: number, limit: number): void {
    this.isDataLoadingSignal.set(true);
    this.clientIdentitySrv.users_GetAll(offset, limit).subscribe({
      next: users => {
        this.datatableSignal.set(new Datatable<UserListDto>(users));
        this.isDataLoadingSignal.set(false);
      },
      error: (err) => {
        this.isDataLoadingSignal.set(false);
        throw err;
      }
    });
  }

  protected onShowCreate(modal: ModalDirective): void {
    this.createPermTree = this.permissionSrv.buildTree([]);
    this.selectedRoleIds = [];
    modal.show();
  }

  protected onCreate(modal: ModalDirective): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isLoadingSignal.set(true);
    const data = new UserCreateDto({
      firstName: this.createForm.value.firstName,
      lastName: this.createForm.value.lastName,
      roleIds: this.selectedRoleIds,
      permissions: this.permissionSrv.extractPermissions(this.createPermTree),
    });

    this.clientIdentitySrv.users_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isLoadingSignal.set(false);
          this.fetchUsers(0, 10);
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
    const data = new UserUpdateDto({
      id: this.editingUser.id,
      firstName: this.updateForm.value.firstName,
      lastName: this.updateForm.value.lastName,
      roleIds: this.selectedRoleIds,
      permissions: this.permissionSrv.extractPermissions(this.updatePermTree),
    });

    this.clientIdentitySrv.users_Update(this.editingUser.id, data)
      .subscribe({
        next: () => {
          this.updateForm.reset();
          this.isLoadingSignal.set(false);
          this.fetchUsers(0, 10);
          modal.hide();
        },
        error: (err) => {
          this.isLoadingSignal.set(false);
          throw err;
        }
      });
  }

  protected async onEdit(item: any, modal: ModalDirective): Promise<void> {
    this.editingUser = await this.clientIdentitySrv.users_Get(item.id).toPromise();
    this.updateForm.patchValue({ firstName: this.editingUser!.firstName, lastName: this.editingUser!.lastName });
    this.selectedRoleIds = this.editingUser!.roles?.map((r: any) => r.id) ?? [];
    this.updatePermTree = this.permissionSrv.buildTree(this.editingUser!.permissions ?? []);
    modal.show();
  }

  onPageChange(event: { currentPage: number; pageSize: number }): void {
    const offset = (event.currentPage - 1) * event.pageSize;
    this.fetchUsers(offset, event.pageSize);
  }

  protected onRoleToggle(roleId: number, checked: boolean): void {
    if (checked) {
      if (!this.selectedRoleIds.includes(roleId)) {
        this.selectedRoleIds.push(roleId);
      }
    } else {
      this.selectedRoleIds = this.selectedRoleIds.filter(id => id !== roleId);
    }
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete user "${item.firstName} ${item.lastName}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientIdentitySrv.users_Delete(item.id)
          .subscribe({ next: () => this.fetchUsers(0, 10) });
      });
  }
}
