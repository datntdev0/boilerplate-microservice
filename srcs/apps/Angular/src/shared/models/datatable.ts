
export interface IDatatable<TData> {
  items: TData[];
  total: number;
  limit: number;
  offset: number;
  totalPages: number;
  currentPage: number;
}

export class Datatable<TData> implements IDatatable<TData> {
  items: TData[];
  total: number;
  limit: number;
  offset: number;

  constructor(data: Partial<IDatatable<TData>> = {}) {
    this.items = data.items || [];
    this.total = data.total || 0;
    this.limit = data.limit || 10;
    this.offset = data.offset || 0;
  }

  get totalPages(): number {
    if (this.limit === 0) return 1;
    return Math.ceil(this.total / this.limit);
  }

  get currentPage(): number {
    if (this.limit === 0) return 1;
    return Math.floor(this.offset / this.limit) + 1;
  }
}