import type { Meta, StoryObj } from '@storybook/web-components-vite';

import { AuthSignInProps, AuthSignIn } from '../../containers/AuthSignIn';

const meta = {
  title: 'Pages/Auth-SignIn',
  render: (args: AuthSignInProps) => AuthSignIn(args),
} satisfies Meta<AuthSignInProps>;

export default meta;
type Story = StoryObj<AuthSignInProps>;

export const SignIn: Story = {
  args: {},
};
