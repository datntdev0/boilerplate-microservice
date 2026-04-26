import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './main.routes';
import { provideGlobalErrorHandler, withRootInitializer } from './root.initializer';

export const mainConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideGlobalErrorHandler(),
    provideHttpClient(withInterceptorsFromDi()),
    provideAppInitializer(withRootInitializer),
  ]
};
