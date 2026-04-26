import { Injectable } from '@angular/core';
import { ALL_PERMISSIONS } from '@shared/models/constants';
import { PermissionNode } from '@shared/models/permission';

export type { PermissionItem, PermissionNode } from '@shared/models/permission';

@Injectable({ providedIn: 'root' })
export class PermissionService {
  /**
   * Builds a tree of PermissionNode from the flat ALL_PERMISSIONS list,
   * marking nodes whose values appear in assignedPermissions as checked.
   */
  buildTree(assignedPermissions: number[] = []): PermissionNode[] {
    const nodeMap = new Map<number, PermissionNode>();

    for (const perm of ALL_PERMISSIONS) {
      nodeMap.set(perm.value, {
        ...perm,
        checked: assignedPermissions.includes(perm.value),
        indeterminate: false,
        collapsed: false,
        children: [],
      });
    }

    const roots: PermissionNode[] = [];
    for (const node of nodeMap.values()) {
      if (node.parentValue === null) {
        roots.push(node);
      } else {
        nodeMap.get(node.parentValue)?.children.push(node);
      }
    }

    for (const root of roots) {
      this.syncParentState(root);
    }

    return roots;
  }

  /**
   * Toggles a parent node and propagates the checked state to all its children.
   */
  onNodeChange(node: PermissionNode, checked: boolean): void {
    node.checked = checked;
    node.indeterminate = false;
    for (const child of node.children) {
      child.checked = checked;
    }
  }

  /**
   * Toggles a child node and recalculates the parent's checked/indeterminate state.
   */
  onChildChange(child: PermissionNode, parent: PermissionNode, checked: boolean): void {
    child.checked = checked;
    this.syncParentState(parent);
  }

  /**
   * Extracts a flat number[] of permission values from the tree,
   * including only nodes where checked === true.
   */
  extractPermissions(tree: PermissionNode[]): number[] {
    const result: number[] = [];
    for (const node of tree) {
      if (node.checked) result.push(node.value);
      for (const child of node.children) {
        if (child.checked) result.push(child.value);
      }
    }
    return result;
  }

  private syncParentState(node: PermissionNode): void {
    if (node.children.length === 0) return;
    const checkedCount = node.children.filter(c => c.checked).length;
    node.checked = checkedCount === node.children.length;
    node.indeterminate = checkedCount > 0 && checkedCount < node.children.length;
  }
}
