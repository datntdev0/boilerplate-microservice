import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

export type ButtonVariant = 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark';

@Component({
  selector: 'sample-button',
  standalone: true,
  imports: [CommonModule],
  template: `<button type="button" [class]="'btn btn-' + variant" [disabled]="disabled">{{ label }}</button>`,
})
export class ButtonComponent {
  @Input() variant: ButtonVariant = 'primary';
  @Input() label: string = 'Button';
  @Input() disabled: boolean = false;
}
