import { NO_ERRORS_SCHEMA, SimpleChange, SimpleChanges } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { FormSelectorOption } from '@components/form-selector/form-selector';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { HeaderComponent } from './header';

const TENANT_KEY = 'datntdev.Microservices.TenantId';

const OPTIONS: FormSelectorOption[] = [
  { value: null, label: 'Host' },
  { value: 1, label: 'Alpha Tenant' },
  { value: 2, label: 'Beta Tenant' },
];

function triggerNgOnChanges(component: HeaderComponent): void {
  const changes: SimpleChanges = {
    tenantOptions: new SimpleChange([], component.tenantOptions, false),
  };
  component.ngOnChanges(changes);
}

describe('Components.HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderComponent],
      providers: [provideRouter([])],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
  });

  afterEach(() => {
    vi.restoreAllMocks();
    sessionStorage.clear();
  });

  it('U10: no stored tenant → auto-selects first option and writes to sessionStorage', () => {
    sessionStorage.removeItem(TENANT_KEY);
    component.tenantOptions = OPTIONS;
    triggerNgOnChanges(component);

    expect(component.selectedTenantId).toBeNull();
    expect(sessionStorage.getItem(TENANT_KEY)).toBe('');
  });

  it('U11: stored tenant exists in list → keeps stored value, does not overwrite sessionStorage', () => {
    sessionStorage.setItem(TENANT_KEY, '1');
    component.tenantOptions = OPTIONS;
    triggerNgOnChanges(component);

    expect(component.selectedTenantId).toBe(1);
    expect(sessionStorage.getItem(TENANT_KEY)).toBe('1');
  });

  it('U12: stored tenant not in list → auto-selects first option and updates sessionStorage', () => {
    sessionStorage.setItem(TENANT_KEY, '99');
    component.tenantOptions = OPTIONS;
    triggerNgOnChanges(component);

    expect(component.selectedTenantId).toBeNull();
    expect(sessionStorage.getItem(TENANT_KEY)).toBe('');
  });

  it('U13: onTenantChange → writes sessionStorage and calls window.location.reload()', () => {
    const reloadMock = vi.fn();
    Object.defineProperty(window, 'location', {
      value: { ...window.location, reload: reloadMock },
      writable: true,
      configurable: true,
    });

    (component as any).onTenantChange(5);

    expect(sessionStorage.getItem(TENANT_KEY)).toBe('5');
    expect(reloadMock).toHaveBeenCalledOnce();
  });

  it('U14: Host option (id=null) maps to { value: null, label: "Host" } and is first in tenantOptions', () => {
    sessionStorage.removeItem(TENANT_KEY);
    const hostFirst: FormSelectorOption[] = [
      { value: null, label: 'Host' },
      { value: 1, label: 'Tenant A' },
    ];
    component.tenantOptions = hostFirst;
    triggerNgOnChanges(component);

    expect(component.tenantOptions[0]).toEqual({ value: null, label: 'Host' });
    expect(component.selectedTenantId).toBeNull();
    expect(sessionStorage.getItem(TENANT_KEY)).toBe('');
  });
});
