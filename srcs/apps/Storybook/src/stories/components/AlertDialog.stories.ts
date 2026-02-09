import type { Meta, StoryObj } from '@storybook/web-components-vite';

import { AlertDialog, AlertDialogProps } from '../../components/AlertDialog';

const meta = {
  title: 'Components/Alert Dialog',
  tags: ['autodocs'],
  parameters: {
    docs: {
      story: {
        height: '300px',
      },
    },
  },
  render: (args: AlertDialogProps) => AlertDialog(args),
  argTypes: {
    type: {
      control: 'select',
      options: ['success', 'danger', 'warning', 'info'],
      description: 'Alert type - determines icon and color scheme',
      table: {
        defaultValue: { summary: 'info' },
      },
    },
    title: {
      control: 'text',
      description: 'Dialog title',
    },
    text: {
      control: 'text',
      description: 'Dialog message/text content',
    },
    confirmButtonText: {
      control: 'text',
      description: 'Confirm button text',
      table: {
        defaultValue: { summary: 'Confirm' },
      },
    },
    confirmButtonVariant: {
      control: 'select',
      options: ['primary', 'secondary', 'success', 'danger', 'warning', 'info'],
      description: 'Confirm button variant',
      table: {
        defaultValue: { summary: 'primary' },
      },
    },
    cancelButtonText: {
      control: 'text',
      description: 'Cancel button text',
      table: {
        defaultValue: { summary: 'Cancel' },
      },
    },
    cancelButtonVariant: {
      control: 'select',
      options: ['primary', 'secondary', 'success', 'danger', 'warning', 'info'],
      description: 'Cancel button variant',
      table: {
        defaultValue: { summary: 'secondary' },
      },
    },
  },
} satisfies Meta<AlertDialogProps>;

export default meta;
type Story = StoryObj<AlertDialogProps>;

// Basic alert types
export const Success: Story = {
  args: {
    type: 'success',
    title: 'Success!',
    text: 'Your action has been completed successfully.',
    confirmButtonText: 'Great!',
    confirmButtonVariant: 'success',
    cancelButtonText: 'Close',
    cancelButtonVariant: 'secondary',
  },
};

export const Danger: Story = {
  args: {
    type: 'danger',
    title: 'Error!',
    text: 'Something went wrong. Please try again.',
    confirmButtonText: 'OK',
    confirmButtonVariant: 'danger',
  },
};

export const Warning: Story = {
  args: {
    type: 'warning',
    title: 'Are you sure?',
    text: 'This action cannot be undone.',
    confirmButtonText: 'Yes, proceed',
    confirmButtonVariant: 'warning',
    cancelButtonText: 'Cancel',
  },
};

export const Info: Story = {
  args: {
    type: 'info',
    title: 'Information',
    text: 'Here is some important information you should know.',
    confirmButtonText: 'Got it',
    confirmButtonVariant: 'info',
  },
};
