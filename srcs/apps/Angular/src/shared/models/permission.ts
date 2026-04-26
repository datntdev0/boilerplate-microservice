export interface PermissionItem {
  value: number;
  name: string;
  parentValue: number | null;
}

export interface PermissionNode extends PermissionItem {
  checked: boolean;
  indeterminate: boolean;
  collapsed: boolean;
  children: PermissionNode[];
}
