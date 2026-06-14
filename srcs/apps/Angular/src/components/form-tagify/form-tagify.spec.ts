import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect } from 'vitest';
import { FormTagifyComponent } from './form-tagify';

describe('Components.FormTagify', () => {
  let component: FormTagifyComponent;
  let fixture: ComponentFixture<FormTagifyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormTagifyComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FormTagifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  // U1 — Enter key creates a chip and clears the input
  it('U1: pressing Enter appends chip and clears input', () => {
    component.inputValue = 'angular';
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    component.onKeyDown(event);

    expect(component.tags).toEqual(['angular']);
    expect(component.inputValue).toBe('');
  });

  // U2 — Comma key creates a chip and clears the input
  it('U2: pressing comma appends chip and clears input', () => {
    component.inputValue = 'react';
    const event = new KeyboardEvent('keydown', { key: ',' });
    component.onKeyDown(event);

    expect(component.tags).toEqual(['react']);
    expect(component.inputValue).toBe('');
  });

  // U3 — Whitespace-only input is ignored
  it('U3: whitespace-only input does not create a chip', () => {
    component.inputValue = '   ';
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    component.onKeyDown(event);

    expect(component.tags).toEqual([]);
    expect(component.inputValue).toBe('');
  });

  // U4 — Duplicate input is silently rejected
  it('U4: duplicate input is silently rejected', () => {
    component.tags = ['angular'];
    component.inputValue = 'angular';
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    component.onKeyDown(event);

    expect(component.tags).toEqual(['angular']);
    expect(component.inputValue).toBe('');
  });

  // U5 — removeTag removes the chip at the given index and calls onChange
  it('U5: removeTag removes chip at index and calls onChange', () => {
    const received: string[][] = [];
    component.registerOnChange((v) => received.push(v));
    component.tags = ['foo', 'bar', 'baz'];

    component.removeTag(1);

    expect(component.tags).toEqual(['foo', 'baz']);
    expect(received).toEqual([['foo', 'baz']]);
  });

  // U6 — writeValue initialises chips from array
  it('U6: writeValue([\'foo\', \'bar\']) initialises two chips', () => {
    component.writeValue(['foo', 'bar']);
    expect(component.tags).toEqual(['foo', 'bar']);
  });

  // U7 — writeValue(null) resets chips to empty array without error
  it('U7: writeValue(null) resets chips to empty array without error', () => {
    component.tags = ['foo'];
    expect(() => component.writeValue(null as any)).not.toThrow();
    expect(component.tags).toEqual([]);
  });

  // U8 — setDisabledState(true) sets disabled flag; interactions are blocked
  it('U8: setDisabledState(true) sets disabled flag and blocks adding tags', () => {
    component.setDisabledState(true);
    expect(component.disabled).toBe(true);

    component.inputValue = 'test';
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    component.onKeyDown(event);

    expect(component.tags).toEqual([]);
  });

  // U9 — onChange is called with updated string[] after adding a tag
  it('U9: onChange is called with updated value after adding a tag', () => {
    const received: string[][] = [];
    component.registerOnChange((v) => received.push(v));
    component.inputValue = 'angular';
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    component.onKeyDown(event);

    expect(received).toEqual([['angular']]);
  });

  // U10 — registerOnChange and registerOnTouched store callbacks
  it('U10: registerOnChange and registerOnTouched store callbacks correctly', () => {
    const changeFn = (v: string[]) => v;
    const touchFn = () => {};

    component.registerOnChange(changeFn);
    component.registerOnTouched(touchFn);

    expect(component['onChange']).toBe(changeFn);
    expect(component['onTouched']).toBe(touchFn);
  });
});
