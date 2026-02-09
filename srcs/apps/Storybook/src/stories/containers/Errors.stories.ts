import type { Meta, StoryObj } from '@storybook/web-components-vite';

import { ErrorPages, ErrorPagesProps } from '../../containers/ErrorPages';

const meta = {
  title: 'Pages/Errors',
  render: (args: ErrorPagesProps) => ErrorPages(args),
  argTypes: {
    title: { control: 'text' },
    description: { control: 'text' },
    imageUrl: { control: 'text' },
    action: { control: 'text' },
  },
} satisfies Meta<ErrorPagesProps>;

export default meta;
type Story = StoryObj<ErrorPagesProps>;

export const Error403: Story = {
  args: {
    title: '403 - Access Forbidden',
    description: 'Sorry, you are not authorized to access this page. Please contact your administrator if you believe this is an error.',
    imageUrl: '/media/images/error403.svg',
    action: 'Return to Home',
  },
};

export const Error404: Story = {
  args: {
    title: '404 - Page Not Found',
    description: 'The page you are looking for does not exist. It may have been moved or deleted.',
    imageUrl: '/media/images/error404.svg',
    action: 'Go Back',
  },
};

export const Error503: Story = {
  args: {
    title: '503 - Service Unavailable',
    description: 'The service is temporarily unavailable. Please try again later.',
    imageUrl: '/media/images/error503.svg',
    action: 'Refresh Page',
  },
};
