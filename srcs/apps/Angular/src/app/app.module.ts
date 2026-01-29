import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppPage } from './app.page';

const routes: Routes = [
  { path: '', component: AppPage }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
})
export default class AppModule { }
