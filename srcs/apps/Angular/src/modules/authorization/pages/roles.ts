import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog.service';
import { Datatable } from '@shared/models/datatable';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { PermissionNode, PermissionService } from '@shared/services/permission.service';
import { RoleCreateDto, RoleListDto, RoleUpdateDto, SrvIdentityClientProxy } from '@shared/proxies/srv-identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './roles.html',
})
export class RolesPage implements OnInit {
  private readonly clientIdentitySrv = inject(SrvIdentityClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);
  private readonly permissionSrv = inject(PermissionService);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  public datatableSignal = signal(new Datatable<RoleListDto>());
  public isLoadingSignal = signal(false);

  editingRole: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  createPermTree: PermissionNode[] = [];
  updatePermTree: PermissionNode[] = [];

  columns: DatatableColumn[] = [
    {
      key: 'name',
      title: 'Role Name',
      template: (item) => `<span class="text-gray-800 text-hover-primary mb-1">${item.name}</span>`,
    },
    {
      key: 'description',
      title: 'Description',
    },
    {
      key: 'createdAt',
      title: 'Created Date',
      template: (item) => this.localDateTimePipe.transform(item.createdAt)
    },
    {
      key: 'updatedAt',
      title: 'Updated Date',
      template: (item) => this.localDateTimePipe.transform(item.updatedAt)
    }
  ];

  ngOnInit(): void {
    this.createForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
    });
    this.updateForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
    });

    this.clientIdentitySrv.roles_GetAll(0, 10).subscribe(
      roles => this.datatableSignal.set(new Datatable<RoleListDto>(roles)));
  }

  protected onShowCreate(modal: ModalDirective): void {
    this.createPermTree = this.permissionSrv.buildTree([]);
    modal.show();
  }

  
  protected async onShowUpdate(item: any, modal: ModalDirective): Promise<void> {
    this.editingRole = await this.clientIdentitySrv.roles_Get(item.id).toPromise();
    this.updateForm.patchValue({ name: item.name, description: item.description });
    this.updatePermTree = this.permissionSrv.buildTree(this.editingRole!.permissions ?? []);
    modal.show();
  }

  protected onCreate(modal: ModalDirective): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isLoadingSignal.set(true);
    const data = new RoleCreateDto({
      name: this.createForm.value.name,
      description: this.createForm.value.description,
      permissions: this.permissionSrv.extractPermissions(this.createPermTree),
    });

    this.clientIdentitySrv.roles_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isLoadingSignal.set(false);
          this.ngOnInit();
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
    const data = new RoleUpdateDto({
      name: this.updateForm.value.name,
      description: this.updateForm.value.description,
      permissions: this.permissionSrv.extractPermissions(this.updatePermTree),
    });

    this.clientIdentitySrv.roles_Update(this.editingRole.id, data)
      .subscribe({
        next: () => {
          this.updateForm.reset();
          this.isLoadingSignal.set(false);
          this.ngOnInit();
          modal.hide();
        },
        error: (err) => {
          this.isLoadingSignal.set(false);
          throw err;
        }
      });
  }

  protected onPermNodeChange(node: PermissionNode, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    this.permissionSrv.onNodeChange(node, checked);
  }

  protected onPermChildChange(child: PermissionNode, parent: PermissionNode, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    this.permissionSrv.onChildChange(child, parent, checked);
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete role "${item.name}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientIdentitySrv.roles_Delete(item.id)
          .subscribe({ next: () => this.ngOnInit() });
      });
  }
}
