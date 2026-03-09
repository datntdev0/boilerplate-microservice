import { ApplicationConfig, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';

import { routes } from './main.routes';
import { withRootInitializer } from './root.initializer';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

export const mainConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptorsFromDi()),
    provideAppInitializer(withRootInitializer),
    provideClientHydration(withEventReplay())
  ]
};
