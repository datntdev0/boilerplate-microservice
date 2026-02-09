import type { Meta, StoryObj } from '@storybook/web-components-vite';

import { AuthSignIn } from '../../containers/AuthSignIn';

const meta = {
  title: 'Pages/Auth - SignIn',
  render: () => AuthSignIn(),
} satisfies Meta;

export default meta;
type Story = StoryObj;

export const SignIn: Story = {};
