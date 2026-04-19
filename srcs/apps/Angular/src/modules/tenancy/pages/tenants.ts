import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog-service';
import { Datatable } from '@shared/models/datatable';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { SrvAdminClientProxy, TenantCreateDto, TenantListDto, TenantUpdateDto } from '@shared/proxies/srv-admin-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './tenants.html',
})
export class TenantsPage implements OnInit {
  private readonly clientAdminSrv = inject(SrvAdminClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  public datatableSignal = signal(new Datatable<TenantListDto>());
  public isLoadingSignal = signal(false);

  editingTenant: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;

  columns: DatatableColumn[] = [
    {
      key: 'name',
      title: 'Tenant Name',
      template: (item) => `<span class="text-gray-800 text-hover-primary mb-1">${item.name}</span>`,
    },
    {
      key: 'organization',
      title: 'Organization',
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
      organization: [''],
    });
    this.updateForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      organization: [''],
    });

    this.clientAdminSrv.tenants_GetAll(0, 10).subscribe(
      tenants => this.datatableSignal.set(new Datatable<TenantListDto>(tenants)));
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
    const data = new TenantUpdateDto({
      name: this.updateForm.value.name,
      organization: this.updateForm.value.organization,
    });

    this.clientAdminSrv.tenants_Update(this.editingTenant.id, data)
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
          .subscribe({ next: () => this.ngOnInit() });
      });
  }
}
