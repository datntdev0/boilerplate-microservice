import { AfterViewInit, ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog.service';
import { Datatable } from '@shared/models/datatable';
import { DateTimePipe } from '@shared/pipes/datetime.pipe';
import { PermissionNode, PermissionService } from '@shared/services/permission.service';
import { RoleListDto, SrvIdentityClientProxy, UserCreateDto, UserListDto, UserUpdateDto } from '@shared/proxies/srv-identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './users.html',
})
export class UsersPage implements OnInit, AfterViewInit {
  private readonly clientIdentitySrv = inject(SrvIdentityClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);
  private readonly permissionSrv = inject(PermissionService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dateTimePipe = new DateTimePipe();

  public datatableSignal = signal(new Datatable<UserListDto>());
  public allRolesSignal = signal(new Datatable<RoleListDto>());
  public isLoadingSignal = signal(false);

  editingUser: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  createPermTree: PermissionNode[] = [];
  updatePermTree: PermissionNode[] = [];
  selectedRoleIds: number[] = [];

  columns: DatatableColumn[] = [
    {
      key: 'firstName',
      title: 'First Name',
      template: (item) => `<span class="text-gray-800 text-hover-primary mb-1">${item.firstName}</span>`,
    },
    {
      key: 'lastName',
      title: 'Last Name',
    },
    {
      key: 'createdAt',
      title: 'Created',
      template: (item) => this.renderDateColumn(item.createdAt)
    },
    {
      key: 'updatedAt',
      title: 'Updated',
      template: (item) => this.renderDateColumn(item.updatedAt)
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

    this.loadUsers();

    this.clientIdentitySrv.roles_GetAll(0, 100).subscribe(
      roles => {
        this.allRolesSignal.set(new Datatable<RoleListDto>(roles));
      }
    );
  }

  ngAfterViewInit(): void {
    this.initializeTooltips();
  }

  private loadUsers(): void {
    this.clientIdentitySrv.users_GetAll(0, 10).subscribe(
      users => {
        this.datatableSignal.set(new Datatable<UserListDto>(users));
        setTimeout(() => this.initializeTooltips(), 0);
      }
    );
  }

  private renderDateColumn(date: Date | string | null | undefined): string {
    if (!date) return '';
    const relativeTime = this.dateTimePipe.relative(date);
    const fullDate = this.dateTimePipe.readable(date);
    return `<span data-bs-toggle="tooltip" data-bs-placement="top" title="${fullDate}">${relativeTime}</span>`;
  }

  private initializeTooltips(): void {
    // Initialize Bootstrap tooltips using vanilla JavaScript
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipTriggerList.forEach(tooltipTriggerEl => {
      // Use Bootstrap's native tooltip via data attributes
      // Bootstrap 5 tooltips are initialized automatically if bootstrap.js is loaded
      // or we can initialize them programmatically without importing Tooltip class
      const tooltip = (window as any).bootstrap?.Tooltip?.getOrCreateInstance(tooltipTriggerEl);
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
          this.loadUsers();
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
          this.loadUsers();
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
    this.cdr.detectChanges();
    modal.show();
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
          .subscribe({ next: () => this.loadUsers() });
      });
  }
}
