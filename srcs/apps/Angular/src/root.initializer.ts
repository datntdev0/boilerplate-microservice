import { inject, Provider } from "@angular/core";
import { Router } from "@angular/router";
import { API_BASE_URL_ADMIN, SrvAdminClientProxy } from "@shared/proxies/srv-admin-proxies";
import { API_BASE_URL_IDENTITY, SrvIdentityClientProxy } from "@shared/proxies/srv-identity-proxies";
import { AuthService } from "@shared/services/auth-service";
import { LoggerService } from "@shared/services/logger-service";
import { environment } from "envs/environment";

export async function withRootInitializer(): Promise<void> {
  const loggerService = inject(LoggerService);
  const authService = inject(AuthService);
  const router = inject(Router);

  loggerService.info('Application is initializing...');

  try {
    await authService.initialize(window.location.pathname);
    loggerService.info('Application initialized successfully');
  } catch (error) {
    loggerService.error('Error during application initialization:', error);
    router.navigate(['/error/500']);
  }
};

export function provideSrvIdentityProxy(): Provider[] {
  return [
    SrvIdentityClientProxy, 
    { provide: API_BASE_URL_IDENTITY, useValue: `${environment.apiUrl}/srv-identity` }
  ];
}

export function provideSrvAdminProxy(): Provider[] {
  return [
    SrvAdminClientProxy,
    { provide: API_BASE_URL_ADMIN, useValue: `${environment.apiUrl}/srv-admin` }
  ];
}