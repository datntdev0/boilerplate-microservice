# Application Sitemap - Pages, Components, Features, and Behaviors

baseUrl: http://localhost:4200
identityUrl: https://localhost:7240
defaultUsername: admin@datntdev.com
defaultPassword: Admin@123

Main Application Pages:
```xml
<pages>
  <page name="Home" path="/">
    <behaviors>
      <behavior name="Default">Redirect to SSO Page at {identityUrl}/auth/signin</behavior>
    </behaviors>
  </page>
  <page name="Dashboard" path="app/dashboard" description="Blank page as placeholder">
    <behaviors>
      <behavior name="Authenticated Access">Redirected from Home page if authenticated</behavior>
    </behaviors>
  </page>
  <page name="Tenants" path="app/tenancy/tenants" description="Page to manage tenants" src="pages/tenants.html">
    <components>
      <component name="Tenant List" type="datatable"/>
      <component name="Tenant List Paginator" type="paginator"/>
      <component name="Create Tenant Form" type="modal"/>
      <component name="Update Tenant Form" type="modal"/>
    </components>
  </page>
  <page name="Users" path="app/authorization/users" description="Page to manage users" src="pages/users.ts">
    <components>
      <component name="User List" type="datatable"/>
      <component name="User List Paginator" type="paginator"/>
      <component name="Create User Form" type="modal"/>
      <component name="Update User Form" type="modal"/>
    </components>
  </page>
  <page name="Roles" path="app/authorization/roles" description="Page to manage roles" src="pages/roles.ts">
    <components>
      <component name="Role List" type="datatable"/>
      <component name="Role List Paginator" type="paginator"/>
      <component name="Create Role Form" type="modal"/>
      <component name="Update Role Form" type="modal"/>
    </components>
  </page>
</pages>
```

Main Application Layouts:
```xml
<layouts>
  <layout name="Main Layout" description="Main application layout with header and content area">
    <components>
      <component name="Header" type="header" src="layout/header/header.html"/>
      <component name="Sidebar" type="sidebar" src="layout/sidebar/sidebar.html"/>
    </components>
  </layout>
</layouts>
```