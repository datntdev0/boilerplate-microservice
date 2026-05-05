import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { DatatableComponent } from './datatable/datatable';
import { DialogComponent } from './dialog/dialog';
import { PaginatorComponent } from './paginator/paginator';
import { SelectorPermissionsComponent } from './selector-permissions/selector-permissions';
import { SelectorRolesComponent } from './selector-roles/selector-roles';
import { DialogService } from './dialog/dialog.service';

@NgModule({
  declarations: [
    DatatableComponent,
    PaginatorComponent,
    DialogComponent,
    SelectorPermissionsComponent,
    SelectorRolesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ModalModule,
  ],
  exports: [
    DatatableComponent,
    PaginatorComponent,
    DialogComponent,
    SelectorPermissionsComponent,
    SelectorRolesComponent,
    ModalModule,
  ],
  providers: [
    DialogService,
    BsModalService,
  ]
})
export class ComponentsModule { }
