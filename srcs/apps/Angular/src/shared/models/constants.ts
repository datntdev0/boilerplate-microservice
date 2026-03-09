import { environment } from "envs/environment";
import { MenuSection } from "./menu";

export const APPLICATION = {
  name: 'datntdev.Microservices',
};

export const AUTH_CONFIGS = {
  // Identity Server URL - update this to match your environment
  authority: environment.ssourl,
  
  // Client ID registered in OpenIddict
  client_id: 'datntdev.Microservices.Public',
  
  // Redirect URIs
  redirect_uri: `${environment.appUrl}/auth/callback`,
  
  // Scopes to request
  scope: 'openid',
  
  // Response type for authorization code flow with PKCE
  response_type: 'code',
};

export const NAVBAR_MENU: MenuSection[] = [
  {
    items: [
      {
        title: 'Dashboard',
        description: 'Overview of your application',
        icon: 'bi bi-bar-chart-line-fill',
        url: '/app/dashboard'
      }
    ]
  },
  {
    heading: 'TENANCY',
    items: [
      {
        title: 'Tenant List',
        description: 'Manage application tenants',
        icon: 'bi bi-buildings-fill',
        url: '/app/tenancy/tenants'
      },
      {
        title: 'Edition Plans',
        description: 'Manage application edition plans',
        icon: 'bi bi-grid-fill',
        url: '/app/tenancy/editions'
      },
      {
        title: 'Subscriptions',
        description: 'Manage application subscriptions',
        icon: 'bi bi-receipt',
        url: '/app/tenancy/subscriptions'
      }
    ]
  },
  {
    heading: 'AUTHORIZATION',
    items: [
      {
        title: 'User Management',
        description: 'Manage application users',
        icon: 'bi bi-people-fill',
        url: '/app/authorization/users'
      },
      {
        title: 'Role Management',
        description: 'Manage application roles',
        icon: 'bi bi-person-vcard-fill',
        url: '/app/authorization/roles'
      }
    ]
  },
  {
    heading: 'ADMINISTRATION',
    items: [
      {
        title: 'Audit Logs',
        description: 'View application audit logs',
        icon: 'bi bi-body-text',
        url: '/app/administration/audit-logs'
      },
      {
        title: 'Configuration',
        description: 'Manage application configuration',
        icon: 'bi bi-gear-fill',
        url: '/app/administration/configuration'
      }
    ]
  },
  {
    heading: 'HELP',
    items: [
      {
        title: 'Getting Started',
        icon: 'bi bi-arrow-right-square-fill',
        url: '#',
        target: '_blank'
      },
      {
        title: 'Documentation',
        icon: 'bi bi-info-square-fill',
        url: '#',
        target: '_blank'
      }
    ]
  }
];