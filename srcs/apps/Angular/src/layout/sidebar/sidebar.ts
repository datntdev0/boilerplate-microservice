import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuSection } from '@shared/models/menu';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.html',
  imports: [RouterModule],
  host: { class: 'app-sidebar flex-column' },
})
export class SidebarComponent {
  @Input() menuItems: MenuSection[] = [];
}
