import type { Meta, StoryObj } from '@storybook/web-components-vite';

import { AuthSignUp } from '../../containers/AuthSignUp';

const meta = {
  title: 'Pages/Auth - SignUp',
  render: () => AuthSignUp(),
} satisfies Meta;

export default meta;
type Story = StoryObj;

export const SignUp: Story = {};
