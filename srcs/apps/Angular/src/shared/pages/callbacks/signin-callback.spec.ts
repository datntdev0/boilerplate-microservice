import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { provideRouter } from '@angular/router';
import { describe, it, beforeEach, afterEach, expect, vi } from 'vitest';

import { SigninCallbackPage } from './signin-callback';
import { AuthService } from '@shared/services/auth-service';
import { LoggerService } from '@shared/services/logger-service';

describe('Pages.SigninCallback', () => {
  let component: SigninCallbackPage;
  let fixture: ComponentFixture<SigninCallbackPage>;
  let authService: ReturnType<typeof vi.mocked<AuthService>>;
  let router: ReturnType<typeof vi.mocked<Router>>;

  beforeEach(async () => {
    const authServiceMock = {
      signInCallback: vi.fn()
    } as unknown as AuthService;
    
    const routerMock = {
      navigate: vi.fn()
    } as unknown as Router;

    await TestBed.configureTestingModule({
      declarations: [SigninCallbackPage],
      providers: [
        provideRouter([]),
        { provide: LoggerService },
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    })
    .compileComponents();

    authService = TestBed.inject(AuthService) as any;
    router = TestBed.inject(Router) as any;
    
    fixture = TestBed.createComponent(SigninCallbackPage);
    component = fixture.componentInstance;
  });

  afterEach(() => {
    vi.clearAllMocks();
    sessionStorage.clear();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to protected route on successful signin', async () => {
    sessionStorage.setItem('redirectUrl', '/protected');
    vi.mocked(authService.signInCallback).mockResolvedValue(null as any);

    await component.ngOnInit();

    expect(authService.signInCallback).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/protected']);
  });

  it('should navigate to root on signin error', async () => {
    vi.mocked(authService.signInCallback).mockRejectedValue('Error');

    await component.ngOnInit();

    expect(authService.signInCallback).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/']);
  });
});
