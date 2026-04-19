import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ComponentsModule } from '@components/components.module';
import { provideSrvAdminProxy } from 'src/root.initializer';
import { TenantsPage } from './pages/tenants';

const routes: Routes = [
  { path: 'tenants', component: TenantsPage },
  { path: '**', redirectTo: '/error/404' }
]

@NgModule({
  declarations: [
    TenantsPage,
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
  ],
  providers: [
    ComponentsModule,
    provideSrvAdminProxy(),
  ],
})
export class TenancyModule { }
