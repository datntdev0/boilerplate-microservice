import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'form-tagify',
  imports: [CommonModule, FormsModule],
  templateUrl: './form-tagify.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormTagifyComponent),
      multi: true,
    },
  ],
})
export class FormTagifyComponent implements ControlValueAccessor {
  @Input() placeholder: string = 'Add a tag…';
  @Input() disabled: boolean = false;

  tags: string[] = [];
  inputValue: string = '';

  private onChange: (value: string[]) => void = () => {};
  private onTouched: () => void = () => {};

  onKeyDown(event: KeyboardEvent): void {
    if (this.disabled) return;

    if (event.key === 'Enter' || event.key === ',') {
      event.preventDefault();
      this.commitInput();
    }
  }

  private commitInput(): void {
    const trimmed = this.inputValue.trim();
    if (!trimmed) {
      this.inputValue = '';
      return;
    }
    if (this.tags.includes(trimmed)) {
      this.inputValue = '';
      return;
    }
    this.tags = [...this.tags, trimmed];
    this.inputValue = '';
    this.onChange(this.tags);
    this.onTouched();
  }

  removeTag(index: number): void {
    if (this.disabled) return;
    this.tags = this.tags.filter((_, i) => i !== index);
    this.onChange(this.tags);
    this.onTouched();
  }

  // ControlValueAccessor implementation

  writeValue(value: string[]): void {
    if (value === null || value === undefined) {
      this.tags = [];
    } else if (Array.isArray(value)) {
      this.tags = [...value];
    } else {
      this.tags = [];
    }
  }

  registerOnChange(fn: (value: string[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
