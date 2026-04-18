import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { DatatableComponent, DatatableColumn } from '../src/components/datatable/datatable';
import { ComponentsModule } from '../src/components/components-module';
import { Component, TemplateRef, ViewChild } from '@angular/core';

// Sample data for stories
const sampleUsers = [
  { id: 1, name: 'John Doe', email: 'john@example.com', role: 'Admin', status: 'Active' },
  { id: 2, name: 'Jane Smith', email: 'jane@example.com', role: 'User', status: 'Active' },
  { id: 3, name: 'Bob Johnson', email: 'bob@example.com', role: 'User', status: 'Inactive' },
  { id: 4, name: 'Alice Williams', email: 'alice@example.com', role: 'Manager', status: 'Active' },
  { id: 5, name: 'Charlie Brown', email: 'charlie@example.com', role: 'User', status: 'Active' },
];

const sampleProducts = [
  { id: 1, name: 'Laptop', category: 'Electronics', price: '$999', stock: 25 },
  { id: 2, name: 'Mouse', category: 'Accessories', price: '$29', stock: 150 },
  { id: 3, name: 'Keyboard', category: 'Accessories', price: '$79', stock: 80 },
  { id: 4, name: 'Monitor', category: 'Electronics', price: '$299', stock: 40 },
  { id: 5, name: 'Headphones', category: 'Audio', price: '$149', stock: 60 },
];

const userColumns: DatatableColumn[] = [
  { key: 'id', title: 'ID', minWidth: '50px', sortable: false },
  { key: 'name', title: 'Name', minWidth: '125px', sortable: true },
  { key: 'email', title: 'Email', minWidth: '175px', sortable: true },
  { key: 'role', title: 'Role', minWidth: '100px', sortable: true },
  {
    key: 'status',
    title: 'Status',
    minWidth: '100px',
    sortable: true,
    template: (item) => {
      const badgeClass = item.status === 'Active' ? 'badge-success' : 'badge-secondary';
      return `<span class="badge ${badgeClass}">${item.status}</span>`;
    }
  },
];

const productColumns: DatatableColumn[] = [
  { key: 'id', title: 'ID', minWidth: '50px', sortable: false },
  { key: 'name', title: 'Product Name', minWidth: '150px', sortable: true },
  { key: 'category', title: 'Category', minWidth: '125px', sortable: true },
  { key: 'price', title: 'Price', minWidth: '100px', sortable: true },
  {
    key: 'stock',
    title: 'Stock',
    minWidth: '100px',
    sortable: true,
    template: (item) => {
      const stockClass = item.stock > 50 ? 'text-success' : item.stock > 20 ? 'text-warning' : 'text-danger';
      return `<span class="${stockClass} fw-bold">${item.stock}</span>`;
    }
  },
];

const meta: Meta<DatatableComponent> = {
  title: 'Components/Datatable',
  component: DatatableComponent,
  decorators: [
    moduleMetadata({
      imports: [ComponentsModule],
    }),
  ],
  tags: ['autodocs'],
  argTypes: {
    data: {
      control: 'object',
      description: 'Array of data items to display in the table',
    },
    columns: {
      control: 'object',
      description: 'Array of column configurations',
    },
    checkboxEnabled: {
      control: 'boolean',
      description: 'Whether to show checkboxes for row selection',
    },
    currentPage: {
      control: { type: 'number', min: 1 },
      description: 'Current page number',
    },
    totalPages: {
      control: { type: 'number', min: 1 },
      description: 'Total number of pages',
    },
    pageChange: {
      action: 'pageChanged',
      description: 'Event emitted when page changes',
    },
  },
  args: {
    checkboxEnabled: true,
    currentPage: 1,
    totalPages: 1,
  },
};

export default meta;
type Story = StoryObj<DatatableComponent>;

export const BasicTable: Story = {
  args: {
    data: sampleUsers,
    columns: [
      { key: 'name', title: 'Name', sortable: true },
      { key: 'email', title: 'Email', sortable: true },
    ],
    checkboxEnabled: false,
    currentPage: 1,
    totalPages: 1,
  },
};

export const WithPagination: Story = {
  args: {
    data: sampleUsers,
    columns: userColumns,
    checkboxEnabled: false,
    currentPage: 2,
    totalPages: 5,
  },
};

export const WithCheckboxes: Story = {
  args: {
    data: sampleUsers,
    columns: userColumns,
    checkboxEnabled: true,
    currentPage: 1,
    totalPages: 1,
  },
};

export const EmptyTable: Story = {
  name: 'Empty State',
  args: {
    data: [],
    columns: userColumns,
    checkboxEnabled: true,
    currentPage: 1,
    totalPages: 1,
  },
  parameters: {
    docs: {
      description: {
        story: 'Shows the empty state with icon and descriptive message. Pagination is automatically hidden when there is no data.',
      },
    },
  },
};

@Component({
  selector: 'datatable-with-actions',
  standalone: true,
  imports: [ComponentsModule],
  template: `
    <app-datatable 
      [data]="data" 
      [columns]="columns"
      [actionsTemplate]="actionsTemplate"
      [checkboxEnabled]="true"
      [currentPage]="1"
      [totalPages]="1">
    </app-datatable>
    
    <ng-template #actionsTemplate let-item>
      <button class="btn btn-icon btn-light btn-active-light-primary w-30px h-30px me-3">
        <i class="fs-6 bi bi-sliders2-vertical"></i>
      </button>
      <button class="btn btn-icon btn-light btn-active-light-primary w-30px h-30px">
        <i class="fs-6 bi bi-x-octagon"></i>
      </button>
    </ng-template>
  `,
})
class DatatableWithActionsComponent {
  @ViewChild('actionsTemplate', { static: true }) actionsTemplate!: TemplateRef<any>;

  data = sampleUsers;
  columns: DatatableColumn[] = [
    { key: 'name', title: 'Name', minWidth: '125px', sortable: true },
    { key: 'email', title: 'Email', minWidth: '175px', sortable: true },
    { key: 'role', title: 'Role', minWidth: '100px', sortable: true },
    {
      key: 'status',
      title: 'Status',
      minWidth: '100px',
      sortable: true,
      template: (item: any) => {
        const badgeClass = item.status === 'Active' ? 'badge-success' : 'badge-secondary';
        return `<span class="badge ${badgeClass}">${item.status}</span>`;
      }
    },
  ];

  handleEdit(item: any) {
    console.log('Edit clicked for:', item);
    alert(`Edit: ${item.name}`);
  }

  handleDelete(item: any) {
    console.log('Delete clicked for:', item);
    alert(`Delete: ${item.name}`);
  }
}

export const WithActionMenu: Story = {
  render: (args) => ({
    props: args,
    template: `
      <datatable-with-actions></datatable-with-actions>
    `,
    moduleMetadata: {
      imports: [DatatableWithActionsComponent, ComponentsModule],
    },
  }),
  parameters: {
    docs: {
      description: {
        story: 'Datatable with action menu containing Edit and Delete options for each row.',
      },
    },
  },
};
