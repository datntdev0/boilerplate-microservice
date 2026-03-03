import { inject } from "@angular/core";
import { LoggerService } from "@shared/services/logger-service";

export async function withRootInitializer(): Promise<void> {
  const loggerService = inject(LoggerService);
  loggerService.info('Application is initializing...');

  loggerService.info('Application initialized successfully');
};