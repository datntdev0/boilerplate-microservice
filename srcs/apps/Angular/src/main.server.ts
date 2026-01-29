import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { mainConfigServer } from './main.config.server';
import RootComponent from './root.component';

const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(RootComponent, mainConfigServer, context);

export default bootstrap;
