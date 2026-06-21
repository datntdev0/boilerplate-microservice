import { Component, Input, Output, EventEmitter, forwardRef, HostListener, ElementRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface FormSelectorOption {
  value: any;
  label: string;
  disabled?: boolean;
}

@Component({
  standalone: false,
  selector: 'app-form-selector',
  templateUrl: './form-selector.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormSelectorComponent),
      multi: true
    }
  ]
})
export class FormSelectorComponent implements ControlValueAccessor {
  @Input() options: FormSelectorOption[] = [];
  @Input() placeholder: string = 'Select an option';
  @Input() multiple: boolean = false;
  @Input() searchable: boolean = true;
  @Input() disabled: boolean = false;
  @Input() clearable: boolean = true;
  @Input() maxHeight: string = '300px';
  @Output() selectionChange = new EventEmitter<any>();

  isOpen: boolean = false;
  searchTerm: string = '';
  selectedValues: any[] = [];
  focusedIndex: number = -1;

  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  constructor(private elementRef: ElementRef) {}

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.closeDropdown();
    }
  }

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (this.disabled) return;

    const filteredOpts = this.filteredOptions;

    switch (event.key) {
      case 'Enter':
        event.preventDefault();
        if (this.isOpen && this.focusedIndex >= 0 && filteredOpts[this.focusedIndex]) {
          this.selectOption(filteredOpts[this.focusedIndex]);
        } else if (!this.isOpen) {
          this.toggleDropdown();
        }
        break;
      case 'ArrowDown':
        event.preventDefault();
        if (!this.isOpen) {
          this.toggleDropdown();
        } else {
          this.focusedIndex = Math.min(this.focusedIndex + 1, filteredOpts.length - 1);
        }
        break;
      case 'ArrowUp':
        event.preventDefault();
        if (this.isOpen) {
          this.focusedIndex = Math.max(this.focusedIndex - 1, 0);
        }
        break;
      case 'Escape':
        event.preventDefault();
        this.closeDropdown();
        break;
      case 'Tab':
        this.closeDropdown();
        break;
    }
  }

  get filteredOptions(): FormSelectorOption[] {
    if (!this.searchTerm) {
      return this.options;
    }
    const term = this.searchTerm.toLowerCase();
    return this.options.filter(opt => 
      opt.label.toLowerCase().includes(term)
    );
  }

  get displayText(): string {
    if (this.selectedValues.length === 0) {
      return '';
    }

    if (this.multiple) {
      const labels = this.selectedValues
        .map(val => this.getLabelForValue(val))
        .filter(label => label);
      return labels.join(', ');
    } else {
      return this.getLabelForValue(this.selectedValues[0]);
    }
  }

  getLabelForValue(value: any): string {
    const option = this.options.find(opt => opt.value === value);
    return option?.label || '';
  }

  toggleDropdown(): void {
    if (this.disabled) return;
    
    this.isOpen = !this.isOpen;
    if (this.isOpen) {
      this.searchTerm = '';
      this.focusedIndex = -1;
      this.onTouched();
    }
  }

  closeDropdown(): void {
    this.isOpen = false;
    this.searchTerm = '';
    this.focusedIndex = -1;
  }

  selectOption(option: FormSelectorOption): void {
    if (option.disabled) return;

    if (this.multiple) {
      const index = this.selectedValues.indexOf(option.value);
      if (index > -1) {
        this.selectedValues.splice(index, 1);
      } else {
        this.selectedValues.push(option.value);
      }
      this.searchTerm = '';
    } else {
      this.selectedValues = [option.value];
      this.closeDropdown();
    }

    this.emitChange();
  }

  isSelected(option: FormSelectorOption): boolean {
    return this.selectedValues.includes(option.value);
  }

  removeValue(value: any, event?: MouseEvent): void {
    if (event) {
      event.stopPropagation();
    }
    
    const index = this.selectedValues.indexOf(value);
    if (index > -1) {
      this.selectedValues.splice(index, 1);
      this.emitChange();
    }
  }

  clearSelection(event: MouseEvent): void {
    event.stopPropagation();
    this.selectedValues = [];
    this.emitChange();
  }

  private emitChange(): void {
    const value = this.multiple ? this.selectedValues : (this.selectedValues[0] || null);
    this.onChange(value);
    this.selectionChange.emit(value);
  }

  // ControlValueAccessor implementation
  writeValue(value: any): void {
    if (value === undefined) {
      this.selectedValues = [];
    } else if (value === null && !this.multiple) {
      // null is a valid selection (e.g. the "Host" tenant with id = null)
      this.selectedValues = [null];
    } else if (this.multiple && Array.isArray(value)) {
      this.selectedValues = [...value];
    } else if (!this.multiple) {
      this.selectedValues = [value];
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onSearchInput(event: Event): void {
    event.stopPropagation();
    this.focusedIndex = -1;
  }
}
