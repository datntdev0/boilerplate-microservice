import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { describe, it, beforeEach, expect, vi } from 'vitest';
import { MainLayout } from './main-layout';
import { AuthService } from '@shared/services/auth-service';
import { signal } from '@angular/core';

// Mock user signal
const mockUserSignal = signal({ 
  profile: { 
    email: 'test@datntdev.com', 
    name: 'Test User' 
  } 
});

// Mock AuthService
const mockAuthService = {
  userSignal: mockUserSignal,
  signOut: vi.fn()
};

describe('Components.MainLayout', () => {
  let component: MainLayout;
  let fixture: ComponentFixture<MainLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainLayout],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MainLayout);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set body attributes on init', () => {
    component.ngOnInit();

    expect(document.body.getAttribute('data-kt-app-layout')).toBe('dark-sidebar');
    expect(document.body.getAttribute('data-kt-app-header-fixed')).toBe('true');
    expect(document.body.getAttribute('data-kt-app-sidebar-fixed')).toBe('true');
    expect(document.body.getAttribute('data-kt-app-sidebar-push-header')).toBe('true');
  });

  it('should call signOut when signOut method is invoked', () => {
    (component as any).signOut();
    expect(mockAuthService.signOut).toHaveBeenCalled();
  });

  it('should return user email from AuthService signal', () => {
    const email = (component as any).userEmailAddress();
    expect(email).toBe('test@datntdev.com');
  });

  it('should return user full name from AuthService signal', () => {
    const fullName = (component as any).userFullName();
    expect(fullName).toBe('Test User');
  });
});