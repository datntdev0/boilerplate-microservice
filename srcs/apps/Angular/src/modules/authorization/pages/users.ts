import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { DialogService } from '@components/dialog/dialog.service';
import { Datatable } from '@shared/models/datatable';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { SrvIdentityClientProxy, UserCreateDto, UserListDto, UserUpdateDto } from '@shared/proxies/srv-identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './users.html',
})
export class UsersPage implements OnInit {
  private readonly clientIdentitySrv = inject(SrvIdentityClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  public datatableSignal = signal(new Datatable<UserListDto>());
  public isLoadingSignal = signal(false);

  editingUser: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;

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
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
    });
    this.updateForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
    });

    this.clientIdentitySrv.users_GetAll(0, 10).subscribe(
      users => this.datatableSignal.set(new Datatable<UserListDto>(users)));
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
    });

    this.clientIdentitySrv.users_Create(data)
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
    const data = new UserUpdateDto({
      firstName: this.updateForm.value.firstName,
      lastName: this.updateForm.value.lastName,
    });

    this.clientIdentitySrv.users_Update(this.editingUser.id, data)
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
    this.editingUser = item;
    this.updateForm.patchValue({ firstName: item.firstName, lastName: item.lastName });
    modal.show();
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete user "${item.firstName} ${item.lastName}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientIdentitySrv.users_Delete(item.id)
          .subscribe({ next: () => this.ngOnInit() });
      });
  }
}
