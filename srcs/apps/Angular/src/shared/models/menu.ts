export interface MenuItem {
  title: string;
  description?: string;
  icon?: string;
  url?: string;
  target?: string;
  children?: MenuItem[];
}

export interface MenuSection {
  heading?: string;
  items: MenuItem[];
}