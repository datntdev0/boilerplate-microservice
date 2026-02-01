import type { Meta, StoryObj } from '@storybook/web-components-vite';
import { fn } from 'storybook/test';

import type { ButtonProps } from '../components/Button';
import { Button } from '../components/Button';

const meta = {
  title: 'Bootstrap/Button',
  tags: ['autodocs'],
  render: (args) => Button(args),
  argTypes: {
    variant: {
      control: { type: 'select' },
      options: ['primary', 'secondary', 'success', 'danger', 'warning', 'info', 'light', 'dark', 'link'],
      description: 'Bootstrap button variant',
    },
    outline: {
      control: 'boolean',
      description: 'Use outline style',
    },
    size: {
      control: { type: 'select' },
      options: ['sm', 'md', 'lg'],
      description: 'Button size',
    },
    disabled: {
      control: 'boolean',
      description: 'Disabled state',
    },
    type: {
      control: { type: 'select' },
      options: ['button', 'submit', 'reset'],
      description: 'Button type attribute',
    },
  },
  args: { onClick: fn() },
} satisfies Meta<ButtonProps>;

export default meta;
type Story = StoryObj<ButtonProps>;

// Variants
export const Primary: Story = {
  args: {
    variant: 'primary',
    label: 'Primary',
  },
};

export const Secondary: Story = {
  args: {
    variant: 'secondary',
    label: 'Secondary',
  },
};

export const Success: Story = {
  args: {
    variant: 'success',
    label: 'Success',
  },
};

export const Danger: Story = {
  args: {
    variant: 'danger',
    label: 'Danger',
  },
};

export const Warning: Story = {
  args: {
    variant: 'warning',
    label: 'Warning',
  },
};

export const Info: Story = {
  args: {
    variant: 'info',
    label: 'Info',
  },
};

export const Light: Story = {
  args: {
    variant: 'light',
    label: 'Light',
  },
};

export const Dark: Story = {
  args: {
    variant: 'dark',
    label: 'Dark',
  },
};

export const Link: Story = {
  args: {
    variant: 'link',
    label: 'Link',
  },
};

// Outline Variants
export const OutlinePrimary: Story = {
  args: {
    variant: 'primary',
    outline: true,
    label: 'Outline Primary',
  },
};

export const OutlineSecondary: Story = {
  args: {
    variant: 'secondary',
    outline: true,
    label: 'Outline Secondary',
  },
};

export const OutlineSuccess: Story = {
  args: {
    variant: 'success',
    outline: true,
    label: 'Outline Success',
  },
};

// Sizes
export const Small: Story = {
  args: {
    size: 'sm',
    label: 'Small Button',
  },
};

export const Medium: Story = {
  args: {
    size: 'md',
    label: 'Medium Button',
  },
};

export const Large: Story = {
  args: {
    size: 'lg',
    label: 'Large Button',
  },
};

// States
export const Disabled: Story = {
  args: {
    variant: 'primary',
    disabled: true,
    label: 'Disabled Button',
  },
};

export const DisabledOutline: Story = {
  args: {
    variant: 'secondary',
    outline: true,
    disabled: true,
    label: 'Disabled Outline',
  },
};
