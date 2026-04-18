import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from './datatable/datatable';
import { PaginatorComponent } from './paginator/paginator';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    DatatableComponent,
    PaginatorComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    DatatableComponent,
    PaginatorComponent,
  ],
  providers: []
})
export class ComponentsModule { }
