import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout';
import { ErrorLayout } from './layout/error-layout';
import { Error403Page } from './shared/pages/error403/error403';
import { Error404Page } from './shared/pages/error404/error404';
import { Error500Page } from './shared/pages/error500/error500';

export const routes: Routes = [
  {
    path: "error", component: ErrorLayout, children: [
      { path: '403', component: Error403Page },
      { path: '404', component: Error404Page },
      { path: '500', component: Error500Page },
      { path: '**', redirectTo: '/error/404' }
    ]
  },
  {
    path: 'app', component: MainLayout, children: [
      { path: 'dashboard', loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule) },
    ]
  },
  { path: '', redirectTo: 'app/dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: '/error/404' }
];
