import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ComponentsModule } from '@components/components.module';
import { provideSrvIdentityProxy } from 'src/root.initializer';
import { RolesPage } from './pages/roles';
import { UsersPage } from './pages/users';

const routes: Routes = [
  { path: 'users', component: UsersPage },
  { path: 'roles', component: RolesPage },
  { path: '**', redirectTo: '/error/404' }
];

@NgModule({
  declarations: [
    UsersPage,
    RolesPage,
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
  ],
  providers: [
    ComponentsModule,
    provideSrvIdentityProxy(),
  ],
})
export class AuthorizationModule { }
