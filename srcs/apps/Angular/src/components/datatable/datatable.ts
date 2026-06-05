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

export interface PageChangeEvent {
  currentPage: number;
  pageSize: number;
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
  @Input() pageSize: number = 10;
  @Input() totalItems: number = 0;
  @Output() pageChange = new EventEmitter<PageChangeEvent>();
  @ContentChildren(DatatableTemplateDirective, { descendants: true }) contentTemplates!: QueryList<DatatableTemplateDirective>;
  
  actionsTemplate?: TemplateRef<any>;
  
  selectedItems: Set<any> = new Set();
  allSelected: boolean = false;

  pageSizeOptions = [
    { value: 5, label: '5' },
    { value: 10, label: '10' },
    { value: 25, label: '25' },
    { value: 50, label: '50' },
    { value: 100, label: '100' },
    { value: 250, label: '250' },
    { value: 500, label: '500' },
  ];

  get totalPages(): number {
    if (this.pageSize === 0 || this.totalItems === 0) return 1;
    return Math.ceil(this.totalItems / this.pageSize);
  }

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
    this.pageChange.emit({ currentPage: page, pageSize: this.pageSize });
  }

  onPageSizeChange(newPageSize: number): void {
    // Reset to page 1 when page size changes
    this.pageChange.emit({ currentPage: 1, pageSize: newPageSize });
  }
}
