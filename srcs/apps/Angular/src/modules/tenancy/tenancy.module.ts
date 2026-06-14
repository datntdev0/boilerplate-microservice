import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ComponentsModule } from '@components/components.module';
import { FormTagifyComponent } from '@components/form-tagify/form-tagify';
import { provideSrvAdminProxy, provideSrvIdentityProxy } from '@shared/proxies/proxy-providers';
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
    FormTagifyComponent,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
  ],
  providers: [
    ComponentsModule,
    provideSrvAdminProxy(),
    provideSrvIdentityProxy(),
  ],
})
export class TenancyModule { }
