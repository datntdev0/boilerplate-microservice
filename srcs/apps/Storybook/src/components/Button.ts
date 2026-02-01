import { html } from 'lit';
import { classMap } from 'lit/directives/class-map.js';

export interface ButtonProps {
  /** Bootstrap button variant */
  variant?: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark' | 'link';
  /** Use outline style */
  outline?: boolean;
  /** Button size */
  size?: 'sm' | 'md' | 'lg';
  /** Disabled state */
  disabled?: boolean;
  /** Button contents */
  label: string;
  /** Optional click handler */
  onClick?: () => void;
  /** Button type attribute */
  type?: 'button' | 'submit' | 'reset';
}

/** Bootstrap-styled button component for user interaction */
export const Button = ({
  variant = 'primary',
  outline = false,
  size = 'md',
  disabled = false,
  label,
  onClick,
  type = 'button',
}: ButtonProps) => {
  const classes = {
    'btn': true,
    [`btn-${outline ? 'outline-' : ''}${variant}`]: true,
    'btn-sm': size === 'sm',
    'btn-lg': size === 'lg',
  };

  return html`
    <button
      type=${type}
      class=${classMap(classes)}
      ?disabled=${disabled}
      @click=${onClick}
    >
      ${label}
    </button>
  `;
};
