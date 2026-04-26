import { TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, afterEach, expect, vi } from 'vitest';
import { User } from 'oidc-client-ts';
import { AuthService } from './auth.service';

// Use vi.hoisted to ensure these are available in the mock factory
const { mockEvents, MockUserManager } = vi.hoisted(() => {
  const mockEvents = {
    addUserLoaded: vi.fn(),
    addUserUnloaded: vi.fn(),
    addAccessTokenExpired: vi.fn(),
    addUserSignedOut: vi.fn(),
  };

  class MockUserManager {
    events = mockEvents;
    metadataService = { getMetadata: vi.fn().mockResolvedValue({}) };
    clearStaleState = vi.fn().mockResolvedValue(undefined);
    getUser = vi.fn().mockResolvedValue(null);
    signinRedirect = vi.fn().mockResolvedValue(undefined);
    signinRedirectCallback = vi.fn().mockResolvedValue({} as any);
    signoutRedirect = vi.fn().mockResolvedValue(undefined);
  }

  return { mockEvents, MockUserManager };
});

// Mock the oidc-client-ts module
vi.mock('oidc-client-ts', () => ({
  UserManager: MockUserManager,
  User: class User {}
}));

describe('Services.AuthService', () => {
  let service: AuthService;
  let userManager: any;

  beforeEach(() => {
    vi.clearAllMocks();

    TestBed.configureTestingModule({
      providers: [AuthService]
    });

    service = TestBed.inject(AuthService);
    userManager = (service as any).userManager;
    
    // Reset all mocks to default state
    userManager.getUser.mockResolvedValue(null);
    userManager.signinRedirect.mockResolvedValue(undefined);
    userManager.signinRedirectCallback.mockResolvedValue({} as User);
    userManager.signoutRedirect.mockResolvedValue(undefined);
    userManager.clearStaleState.mockResolvedValue(undefined);
    userManager.metadataService.getMetadata.mockResolvedValue({});
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize correctly', async () => {
    const mockUser = { expired: false } as User;
    userManager.getUser.mockResolvedValue(mockUser);

    await service.initialize('/dashboard');

    expect(userManager.clearStaleState).toHaveBeenCalled();
    expect(userManager.metadataService.getMetadata).toHaveBeenCalled();
    expect(userManager.getUser).toHaveBeenCalled();
    expect(service.userSignal()).toBe(mockUser);
  });

  it('should call signinRedirect with correct arguments', async () => {
    const args = { state: { returnUrl: '/home' } };

    await service.signIn(args);

    expect(userManager.signinRedirect).toHaveBeenCalledWith(args);
  });

  it('should call signinRedirectCallback and return user', async () => {
    const mockUser = {} as User;
    userManager.signinRedirectCallback.mockResolvedValue(mockUser);

    const result = await service.signInCallback();

    expect(userManager.signinRedirectCallback).toHaveBeenCalled();
    expect(result).toBe(mockUser);
  });

  it('should call signoutRedirect with correct arguments', async () => {
    const args = { state: { returnUrl: '/logout' } };

    await service.signOut(args);

    expect(userManager.signoutRedirect).toHaveBeenCalledWith(args);
  });
});
