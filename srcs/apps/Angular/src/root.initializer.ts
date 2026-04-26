import { ErrorHandler, inject, Injectable, Injector, NgZone, provideBrowserGlobalErrorListeners, Provider } from "@angular/core";
import { Router } from "@angular/router";
import { ToastService } from '@components/toast/toast.service';
import { ErrorResponse } from '@shared/models/proxies';
import { API_BASE_URL_ADMIN, SrvAdminClientProxy } from "@shared/proxies/srv-admin-proxies";
import { API_BASE_URL_IDENTITY, SrvIdentityClientProxy } from "@shared/proxies/srv-identity-proxies";
import { AuthService } from "@shared/services/auth.service";
import { LoggerService } from "@shared/services/logger.service";
import { environment } from "envs/environment";

@Injectable({ providedIn: 'root' })
class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector, private zone: NgZone) {}

  handleError(error: any): void {

    if (error instanceof ErrorResponse) {
      this.zone.run(() => {
        const toastService = this.injector.get(ToastService);
        toastService.error(`Error ${error.statusCode}`, error.message);
      });
    }
    else {
      // Log the error to the console (or send it to a logging server)
      console.error('An unexpected error occurred:', error);
    }
  }
}

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

export function provideGlobalErrorHandler() {
  return [
    provideBrowserGlobalErrorListeners(),
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ];
}