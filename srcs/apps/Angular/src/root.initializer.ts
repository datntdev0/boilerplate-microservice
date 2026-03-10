import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "@shared/services/auth-service";
import { LoggerService } from "@shared/services/logger-service";

export async function withRootInitializer(): Promise<void> {
  const loggerService = inject(LoggerService);
  const authService = inject(AuthService);
  const router = inject(Router);

  loggerService.info('Application is initializing...');

  try {
    await authService.initialize(router.url);
    loggerService.info('Application initialized successfully');
  } catch (error) {
    loggerService.error('Error during application initialization:', error);
    router.navigate(['/error/500']);
  }
};