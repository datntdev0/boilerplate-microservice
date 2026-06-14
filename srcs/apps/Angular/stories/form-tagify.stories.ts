import type { Meta, StoryObj } from '@storybook/angular';
import { FormTagifyComponent } from '../src/components/form-tagify/form-tagify';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { JsonPipe } from '@angular/common';

const meta: Meta<FormTagifyComponent> = {
  title: 'Components/Form Tagify',
  component: FormTagifyComponent,
  tags: ['autodocs'],
  argTypes: {
    placeholder: {
      control: 'text',
      description: 'Placeholder text shown when the input is empty',
    },
    disabled: {
      control: 'boolean',
      description: 'Disable the component (read-only tag list)',
    },
  },
};

export default meta;
type Story = StoryObj<FormTagifyComponent>;

// Story 1: Default — empty component with default placeholder
export const Default: Story = {
  args: {
    placeholder: 'Add a tag…',
    disabled: false,
  },
};

// Story 2: Pre-populated — initial tags set via writeValue / FormControl
export const PrePopulated: Story = {
  render: () => {
    const ctrl = new FormControl<string[]>(['foo', 'bar']);
    return {
      props: { ctrl },
      moduleMetadata: {
        imports: [FormTagifyComponent, ReactiveFormsModule, JsonPipe],
      },
      template: `
        <form-tagify [formControl]="ctrl" placeholder="Add a tag…"></form-tagify>
        <div class="mt-2 text-muted small">Current value: {{ ctrl.value | json }}</div>
      `,
    };
  },
};

// Story 3: Disabled — read-only tag list, input non-interactive
export const Disabled: Story = {
  render: () => {
    const ctrl = new FormControl<string[]>({ value: ['angular', 'typescript'], disabled: true });
    return {
      props: { ctrl },
      moduleMetadata: {
        imports: [FormTagifyComponent, ReactiveFormsModule, JsonPipe],
      },
      template: `
        <form-tagify [formControl]="ctrl" placeholder="Add a tag…"></form-tagify>
        <div class="mt-2 text-muted small">Current value: {{ ctrl.value | json }}</div>
      `,
    };
  },
};

// Story 4: Placeholder — custom placeholder, no tags
export const Placeholder: Story = {
  args: {
    placeholder: 'Type and press Enter or comma…',
    disabled: false,
  },
};
