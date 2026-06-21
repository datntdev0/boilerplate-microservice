import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { AuthService } from '@shared/services/auth.service';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { authInterceptor } from './auth.interceptor';

// Prevent real oidc-client-ts from loading when auth.service.ts is imported as a DI token
vi.mock('oidc-client-ts', () => ({
  UserManager: class { events = { addUserLoaded: vi.fn(), addUserUnloaded: vi.fn(), addAccessTokenExpired: vi.fn(), addUserSignedOut: vi.fn() }; },
  User: class {},
}));

const TENANT_KEY = 'datntdev.Microservices.TenantId';
const TENANT_HEADER = 'x-datntdev.microservices-tenantid';

describe('Interceptors.authInterceptor', () => {
  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;
  const mockUserSignal = signal<any>(null);

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        { provide: AuthService, useValue: { userSignal: mockUserSignal } },
      ],
    });

    httpClient = TestBed.inject(HttpClient);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTesting.verify();
    sessionStorage.clear();
  });

  it('U07: sessionStorage has "42" → request includes tenant header', () => {
    sessionStorage.setItem(TENANT_KEY, '42');
    httpClient.get('/api/test').subscribe();
    const req = httpTesting.expectOne('/api/test');
    expect(req.request.headers.get(TENANT_HEADER)).toBe('42');
    req.flush({});
  });

  it('U08: sessionStorage has "" (Host context) → tenant header absent', () => {
    sessionStorage.setItem(TENANT_KEY, '');
    httpClient.get('/api/test').subscribe();
    const req = httpTesting.expectOne('/api/test');
    expect(req.request.headers.has(TENANT_HEADER)).toBe(false);
    req.flush({});
  });

  it('U09: sessionStorage key absent → tenant header absent', () => {
    sessionStorage.removeItem(TENANT_KEY);
    httpClient.get('/api/test').subscribe();
    const req = httpTesting.expectOne('/api/test');
    expect(req.request.headers.has(TENANT_HEADER)).toBe(false);
    req.flush({});
  });
});
