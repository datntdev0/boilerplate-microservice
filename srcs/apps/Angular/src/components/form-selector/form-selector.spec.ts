import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { FormSelectorComponent } from './form-selector';

describe('FormSelectorComponent', () => {
  let component: FormSelectorComponent;
  let fixture: ComponentFixture<FormSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FormSelectorComponent],
      imports: [FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(FormSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should filter options based on search term', () => {
    component.options = [
      { value: 1, label: 'Apple' },
      { value: 2, label: 'Banana' },
      { value: 3, label: 'Cherry' }
    ];
    component.searchTerm = 'app';
    
    const filtered = component.filteredOptions;
    expect(filtered.length).toBe(1);
    expect(filtered[0].label).toBe('Apple');
  });

  it('should select option in single mode', () => {
    component.multiple = false;
    const option = { value: 1, label: 'Option 1' };
    
    component.selectOption(option);
    
    expect(component.selectedValues).toEqual([1]);
    expect(component.isOpen).toBe(false);
  });

  it('should toggle option in multiple mode', () => {
    component.multiple = true;
    const option1 = { value: 1, label: 'Option 1' };
    const option2 = { value: 2, label: 'Option 2' };
    
    component.selectOption(option1);
    expect(component.selectedValues).toEqual([1]);
    
    component.selectOption(option2);
    expect(component.selectedValues).toEqual([1, 2]);
    
    component.selectOption(option1);
    expect(component.selectedValues).toEqual([2]);
  });

  it('should clear selection', () => {
    component.selectedValues = [1, 2, 3];
    const event = new MouseEvent('click');
    
    component.clearSelection(event);
    
    expect(component.selectedValues).toEqual([]);
  });

  it('should display correct text for single selection', () => {
    component.multiple = false;
    component.options = [{ value: 1, label: 'Selected Option' }];
    component.selectedValues = [1];
    
    expect(component.displayText).toBe('Selected Option');
  });

  it('should display correct text for multiple selection', () => {
    component.multiple = true;
    component.options = [
      { value: 1, label: 'Option 1' },
      { value: 2, label: 'Option 2' }
    ];
    component.selectedValues = [1, 2];
    
    expect(component.displayText).toBe('Option 1, Option 2');
  });
});
