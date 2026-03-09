import { Component, DOCUMENT, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { APPLICATION, NAVBAR_MENU } from '@shared/models/constants';
import { MenuSection } from '@shared/models/menu';
import { HeaderComponent } from './header/header';
import { SidebarComponent } from './sidebar/sidebar';

@Component({
  selector: 'main-layout',
  imports: [RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './main-layout.html',
})
export class MainLayout implements OnInit {
  private readonly document: Document = inject(DOCUMENT);
  
  public applicationName: string = APPLICATION.name;
  public menuItems: MenuSection[] = NAVBAR_MENU;

  public ngOnInit(): void {
    this.document.body.setAttribute('data-kt-app-layout', 'dark-sidebar');
    this.document.body.setAttribute('data-kt-app-header-fixed', 'true');
    this.document.body.setAttribute('data-kt-app-sidebar-fixed', 'true');
    this.document.body.setAttribute('data-kt-app-sidebar-push-header', 'true');
  }

}
