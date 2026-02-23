import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'sample-button',
  standalone: true,
  imports: [CommonModule],
  template: `<button type="button" class="btn btn-primary">Button</button>`,
})
export class ButtonComponent {}
