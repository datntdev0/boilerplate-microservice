import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { DatatableComponent } from './datatable/datatable';
import { DatatableTemplateDirective } from './datatable/datatable-template.directive';
import { DialogComponent } from './dialog/dialog';
import { FormSelectorComponent } from './form-selector/form-selector';
import { PaginatorComponent } from './paginator/paginator';
import { SelectorPermissionsComponent } from './selector-permissions/selector-permissions';
import { SelectorRolesComponent } from './selector-roles/selector-roles';
import { DialogService } from './dialog/dialog.service';

@NgModule({
  declarations: [
    DatatableComponent,
    DatatableTemplateDirective,
    PaginatorComponent,
    DialogComponent,
    FormSelectorComponent,
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
    DatatableTemplateDirective,
    PaginatorComponent,
    DialogComponent,
    FormSelectorComponent,
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
