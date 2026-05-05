import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RoleListDto } from '@shared/proxies/srv-identity-proxies';

@Component({
  standalone: false,
  selector: 'app-selector-roles',
  templateUrl: './selector-roles.html',
})
export class SelectorRolesComponent {
  @Input() roles: RoleListDto[] = [];
  @Input() selectedRoleIds: number[] = [];
  @Input() idPrefix: string = 'role';
  @Output() roleToggle = new EventEmitter<{ roleId: number; checked: boolean }>();

  onRoleToggle(roleId: number, checked: boolean): void {
    this.roleToggle.emit({ roleId, checked });
  }

  isSelected(roleId: number | undefined): boolean {
    return roleId !== undefined && this.selectedRoleIds.includes(roleId);
  }
}
