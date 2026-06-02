import { Component, Input, Output, EventEmitter, TemplateRef, ContentChildren, QueryList, AfterContentInit } from '@angular/core';
import { DatatableTemplateDirective } from './datatable-template.directive';

export interface DatatableColumn {
  key: string;
  title: string;
  minWidth?: string;
  sortable?: boolean;
  datatype?: 'string' | 'date' | 'numeric';
  template?: (item: any) => string;
  cellTemplate?: TemplateRef<{ $implicit: any }>;
}

@Component({
  standalone: false,
  selector: 'app-datatable',
  templateUrl: './datatable.html',
})
export class DatatableComponent implements AfterContentInit {
  @Input() data: any[] = [];
  @Input() columns: DatatableColumn[] = [];
  @Input() checkboxEnabled: boolean = true;
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 1;
  @Output() pageChange = new EventEmitter<number>();
  @ContentChildren(DatatableTemplateDirective, { descendants: true }) contentTemplates!: QueryList<DatatableTemplateDirective>;
  
  actionsTemplate?: TemplateRef<any>;
  
  selectedItems: Set<any> = new Set();
  allSelected: boolean = false;

  ngAfterContentInit(): void {
    // Map content templates to columns by key
    this.contentTemplates?.forEach(templateDirective => {
      if (templateDirective.column === 'actions') {
        // Special handling for actions template
        this.actionsTemplate = templateDirective.template;
      } else {
        const col = this.columns.find(c => c.key === templateDirective.column);
        if (col) {
          col.cellTemplate = templateDirective.template;
        }
      }
    });
  }

  toggleSelectAll() {
    if (this.allSelected) {
      this.data.forEach(item => this.selectedItems.add(item));
    } else {
      this.selectedItems.clear();
    }
  }

  toggleSelectItem(item: any) {
    if (this.selectedItems.has(item)) {
      this.selectedItems.delete(item);
    } else {
      this.selectedItems.add(item);
    }
    this.allSelected = this.selectedItems.size === this.data.length;
  }

  isSelected(item: any): boolean {
    return this.selectedItems.has(item);
  }

  hasCellTemplate(column: DatatableColumn): boolean {
    return !!column.cellTemplate;
  }

  shouldUseDateTimeTemplate(column: DatatableColumn): boolean {
    return column.datatype === 'date' && !this.hasCellTemplate(column);
  }

  shouldUseDefaultCellTemplate(column: DatatableColumn): boolean {
    return !this.hasCellTemplate(column) && column.datatype !== 'date';
  }

  getCellValue(item: any, column: DatatableColumn): string {
    if (column.template) {
      return column.template(item);
    }
    return item[column.key] || '';
  }

  getDateValue(item: any, column: DatatableColumn): any {
    return item[column.key];
  }

  getColspan(): number {
    let count = this.columns.length;
    if (this.checkboxEnabled) count++;
    if (this.actionsTemplate) count++;
    return count;
  }

  onPageChange(page: number): void {
    this.pageChange.emit(page);
  }
}
