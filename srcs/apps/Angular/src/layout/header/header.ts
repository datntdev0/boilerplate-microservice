import { CommonModule } from '@angular/common';
import { Component, DOCUMENT, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MenuSection } from '@shared/models/menu';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  imports: [CommonModule, PopoverModule],
  templateUrl: './header.html'
})
export class HeaderComponent implements OnInit {
  
  private readonly document: Document = inject(DOCUMENT);
  private readonly router: Router = inject(Router);

  @Input() applicationName: string = 'Angular App';
  @Input() avatarUrl: string = 'images/avatar-default.svg';
  @Input() emailAddress: string = 'developer@datntdev.com';
  @Input() fullName: string = 'Developer User';
  @Input() menuItems: MenuSection[] = [];

  @Output() onClickSignOut = new EventEmitter<void>();

  public pageTitle: string | undefined
  public pageDescription: string | undefined

  protected signOut(): void {
    this.onClickSignOut.emit();
  }

  public ngOnInit(): void {
    // Set initial page info
    this.updatePageInfo(this.router.url);

    // Subscribe to route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => this.updatePageInfo(event.urlAfterRedirects));
  }

  private updatePageInfo(url: string): void {
    // Find matching menu item by URL
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
