import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'app', loadChildren: () => import('./app/app.module').then(m => m.default) },
  { path: '', redirectTo: 'app', pathMatch: 'full' },
];
