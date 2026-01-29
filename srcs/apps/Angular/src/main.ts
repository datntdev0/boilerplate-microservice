import { bootstrapApplication } from '@angular/platform-browser';
import { mainConfig } from './main.config';
import RootComponent from './root.component';

bootstrapApplication(RootComponent, mainConfig)
  .catch((err) => console.error(err));
