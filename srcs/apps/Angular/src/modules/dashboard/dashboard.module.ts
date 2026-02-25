import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardPage } from './pages/dashboard';

const routes: Routes = [
  { path: '', component: DashboardPage },
  { path: '**', redirectTo: '/error/404' }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)]
})
export class DashboardModule { }
