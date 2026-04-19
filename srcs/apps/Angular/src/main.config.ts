import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideGlobalErrorHandler } from '@shared/services/error-handler';
import { routes } from './main.routes';
import { withRootInitializer } from './root.initializer';

export const mainConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideGlobalErrorHandler(),
    provideHttpClient(withInterceptorsFromDi()),
    provideAppInitializer(withRootInitializer),
  ]
};
