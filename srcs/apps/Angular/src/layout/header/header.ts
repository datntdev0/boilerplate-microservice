import { CommonModule } from '@angular/common';
import { Component, DOCUMENT, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { ComponentsModule } from '@components/components.module';
import { FormSelectorOption } from '@components/form-selector/form-selector';
import { APPLICATION } from '@shared/models/constants';
import { MenuSection } from '@shared/models/menu';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { filter } from 'rxjs/operators';

const TENANT_ID_KEY = `${APPLICATION.name}.TenantId`;

@Component({
  selector: 'app-header',
  imports: [CommonModule, PopoverModule, ComponentsModule, FormsModule],
  templateUrl: './header.html',
  styles: [`
    ::ng-deep .tenant-selector .form-selector-single-value {
      overflow: hidden;
      white-space: nowrap;
      text-overflow: ellipsis;
      display: block;
      max-width: 160px;
    }
  `]
})
export class HeaderComponent implements OnInit, OnChanges {

  private readonly document: Document = inject(DOCUMENT);
  private readonly router: Router = inject(Router);

  @Input() applicationName: string = 'Angular App';
  @Input() avatarUrl: string = 'images/avatar-default.svg';
  @Input() emailAddress: string = 'developer@datntdev.com';
  @Input() fullName: string = 'Developer User';
  @Input() menuItems: MenuSection[] = [];
  @Input() tenantOptions: FormSelectorOption[] = [];

  @Output() onClickSignOut = new EventEmitter<void>();

  public pageTitle: string | undefined;
  public pageDescription: string | undefined;
  public selectedTenantId: any = null;

  get selectedTenantLabel(): string {
    return this.tenantOptions.find(o => o.value === this.selectedTenantId)?.label ?? '';
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['tenantOptions'] && this.tenantOptions.length > 0) {
      const stored = this.getStoredTenantId();
      const inList = stored !== undefined && this.tenantOptions.some(o => o.value === stored);
      this.selectedTenantId = inList ? stored : this.tenantOptions[0].value;
      if (!inList) this.persistTenantId(this.selectedTenantId);
    }
  }

  public ngOnInit(): void {
    this.updatePageInfo(this.router.url);
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => this.updatePageInfo(event.urlAfterRedirects));
  }

  protected signOut(): void {
    this.onClickSignOut.emit();
  }

  protected onTenantChange(value: any): void {
    this.persistTenantId(value);
    window.location.reload();
  }

  private getStoredTenantId(): any {
    const stored = sessionStorage.getItem(TENANT_ID_KEY);
    if (stored === null) return undefined;
    if (stored === '') return null;
    const num = Number(stored);
    return isNaN(num) ? null : num;
  }

  private persistTenantId(value: any): void {
    sessionStorage.setItem(TENANT_ID_KEY, value == null ? '' : String(value));
  }

  private updatePageInfo(url: string): void {
    for (const section of this.menuItems) {
      for (const item of section.items) {
        if (item.url && url.includes(item.url)) {
          this.pageTitle = item.title;
          this.pageDescription = item.description;
          this.updateHeadTitle(item.title);
          return;
        }
      }
    }
  }

  private updateHeadTitle(title: string): void {
    this.document.title = `${this.applicationName} - ${title}`;
  }
}
