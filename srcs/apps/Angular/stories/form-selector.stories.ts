import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { FormSelectorComponent } from '../src/components/form-selector/form-selector';
import { ComponentsModule } from '../src/components/components.module';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

// Sample data for stories
const countriesOptions = [
  { value: 'us', label: 'United States' },
  { value: 'uk', label: 'United Kingdom' },
  { value: 'ca', label: 'Canada' },
  { value: 'au', label: 'Australia' },
  { value: 'de', label: 'Germany' },
  { value: 'fr', label: 'France' },
  { value: 'jp', label: 'Japan' },
  { value: 'cn', label: 'China' },
  { value: 'in', label: 'India' },
  { value: 'br', label: 'Brazil' },
];

const fruitsOptions = [
  { value: 'apple', label: 'Apple' },
  { value: 'banana', label: 'Banana' },
  { value: 'cherry', label: 'Cherry' },
  { value: 'date', label: 'Date' },
  { value: 'elderberry', label: 'Elderberry' },
  { value: 'fig', label: 'Fig' },
  { value: 'grape', label: 'Grape' },
  { value: 'honeydew', label: 'Honeydew' },
];

const rolesOptions = [
  { value: 'admin', label: 'Administrator' },
  { value: 'manager', label: 'Manager' },
  { value: 'user', label: 'User' },
  { value: 'guest', label: 'Guest', disabled: true },
  { value: 'moderator', label: 'Moderator' },
];

const meta: Meta<FormSelectorComponent> = {
  title: 'Components/Form Selector',
  component: FormSelectorComponent,
  decorators: [
    moduleMetadata({
      imports: [ComponentsModule, FormsModule],
    }),
  ],
  tags: ['autodocs'],
  argTypes: {
    options: {
      control: 'object',
      description: 'Array of options to display in the selector',
    },
    placeholder: {
      control: 'text',
      description: 'Placeholder text when no option is selected',
    },
    multiple: {
      control: 'boolean',
      description: 'Enable multiple selection mode',
    },
    searchable: {
      control: 'boolean',
      description: 'Enable search functionality',
    },
    disabled: {
      control: 'boolean',
      description: 'Disable the selector',
    },
    clearable: {
      control: 'boolean',
      description: 'Show clear button to remove selection',
    },
    maxHeight: {
      control: 'text',
      description: 'Maximum height of the dropdown',
    },
  },
};

export default meta;
type Story = StoryObj<FormSelectorComponent>;

// Basic single selection
export const SingleSelection: Story = {
  args: {
    options: countriesOptions,
    placeholder: 'Select a country',
    multiple: false,
    searchable: true,
    clearable: true,
    disabled: false,
  },
};

// Multiple selection
export const MultipleSelection: Story = {
  args: {
    options: fruitsOptions,
    placeholder: 'Select fruits',
    multiple: true,
    searchable: true,
    clearable: true,
    disabled: false,
  },
};

// With disabled options
export const WithDisabledOptions: Story = {
  args: {
    options: rolesOptions,
    placeholder: 'Select a role',
    multiple: false,
    searchable: true,
    clearable: true,
    disabled: false,
  },
};


// Custom max height
export const CustomMaxHeight: Story = {
  args: {
    options: countriesOptions,
    placeholder: 'Select a country',
    multiple: false,
    searchable: true,
    clearable: true,
    disabled: false,
    maxHeight: '150px',
  },
};

// Without search
export const WithoutSearch: Story = {
  args: {
    options: rolesOptions,
    placeholder: 'Select a role',
    multiple: false,
    searchable: false,
    clearable: true,
    disabled: false,
  },
};

// Not clearable
export const NotClearable: Story = {
  args: {
    options: countriesOptions,
    placeholder: 'Select a country',
    multiple: false,
    searchable: true,
    clearable: false,
    disabled: false,
  },
};

// Disabled state
export const Disabled: Story = {
  args: {
    options: countriesOptions,
    placeholder: 'Select a country',
    multiple: false,
    searchable: true,
    clearable: true,
    disabled: true,
  },
};

