import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRouter } from '@angular/router';
import { authInterceptor } from '@shared/interceptors/auth.interceptor';
import { routes } from './main.routes';
import { provideGlobalErrorHandler, withRootInitializer } from './root.initializer';

export const mainConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideGlobalErrorHandler(),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAppInitializer(withRootInitializer),
  ]
};
