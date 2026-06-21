import { Component, DOCUMENT, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormSelectorOption } from '@components/form-selector/form-selector';
import { APPLICATION, NAVBAR_MENU } from '@shared/models/constants';
import { MenuSection } from '@shared/models/menu';
import { SrvIdentityClientProxy } from '@shared/proxies/srv-identity-proxies';
import { AuthService } from '@shared/services/auth.service';
import { HeaderComponent } from './header/header';
import { SidebarComponent } from './sidebar/sidebar';

@Component({
  selector: 'main-layout',
  imports: [RouterModule, HeaderComponent, SidebarComponent],
  host: { "class": "d-flex flex-column flex-root" },
  templateUrl: './main-layout.html',
})
export class MainLayout implements OnInit {
  private readonly document: Document = inject(DOCUMENT);
  private readonly authService = inject(AuthService);
  private readonly identityProxy = inject(SrvIdentityClientProxy);

  public applicationName: string = APPLICATION.name;
  public menuItems: MenuSection[] = NAVBAR_MENU;
  public tenantOptions: FormSelectorOption[] = [];

  public ngOnInit(): void {
    this.document.body.setAttribute('data-kt-app-layout', 'dark-sidebar');
    this.document.body.setAttribute('data-kt-app-header-fixed', 'true');
    this.document.body.setAttribute('data-kt-app-sidebar-fixed', 'true');
    this.document.body.setAttribute('data-kt-app-sidebar-push-header', 'true');

    if (this.authService.userSignal()) {
      this.identityProxy.identities_GetSession().subscribe(session => {
        this.tenantOptions = (session.user?.tenants ?? []).map(t => ({
          value: t.id ?? null,
          label: t.name ?? '',
        }));
      });
    }
  }

  protected userEmailAddress = () => this.authService.userSignal()?.profile.email ?? '';
  protected userFullName = () => this.authService.userSignal()?.profile.name ?? '';

  protected signOut(): void {
    this.authService.signOut({ post_logout_redirect_uri: this.document.location.origin });
  }
}
