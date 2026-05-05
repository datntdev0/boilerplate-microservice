import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { PermissionNode, PermissionService } from '@shared/services/permission.service';

@Component({
  standalone: false,
  selector: 'app-selector-permissions',
  templateUrl: './selector-permissions.html',
})
export class SelectorPermissionsComponent {
  private readonly permissionSrv = inject(PermissionService);

  @Input() permissionTree: PermissionNode[] = [];
  @Input() idPrefix: string = 'perm';

  onNodeChange(node: PermissionNode, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    this.permissionSrv.onNodeChange(node, checked);
  }

  onChildChange(child: PermissionNode, parent: PermissionNode, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    this.permissionSrv.onChildChange(child, parent, checked);
  }
}
