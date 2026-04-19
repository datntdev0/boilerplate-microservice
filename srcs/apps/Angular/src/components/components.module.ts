import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { DatatableComponent } from './datatable/datatable';
import { DialogComponent } from './dialog/dialog';
import { PaginatorComponent } from './paginator/paginator';
import { DialogService } from './dialog/dialog-service';

@NgModule({
  declarations: [
    DatatableComponent,
    PaginatorComponent,
    DialogComponent,
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
    ModalModule,
  ],
  providers: [
    DialogService,
    BsModalService,
  ]
})
export class ComponentsModule { }
