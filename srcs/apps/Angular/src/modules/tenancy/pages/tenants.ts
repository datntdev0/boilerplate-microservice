import { AfterViewInit, Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog.service';
import { Datatable } from '@shared/models/datatable';
import { SrvAdminClientProxy, TenantCreateDto, TenantListDto, TenantUpdateDto } from '@shared/proxies/srv-admin-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './tenants.html',
})
export class TenantsPage implements OnInit, AfterViewInit {
  private readonly clientAdminSrv = inject(SrvAdminClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);

  public datatableSignal = signal(new Datatable<TenantListDto>());
  public isLoadingSignal = signal(false);
  public isDataLoadingSignal = signal(false);

  editingTenant: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;

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

  onPageChange(page: number): void {
    const limit = this.datatableSignal().limit;
    const offset = (page - 1) * limit;
    this.fetchTenants(offset, limit);
  }

  protected onEdit(item: any, modal: ModalDirective): void {
    this.editingTenant = item;
    this.updateForm.patchValue({ name: item.name });
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
