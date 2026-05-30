# Application Sitemap - Pages, Components, Features, and Behaviors

baseUrl: http://localhost:4200
identityUrl: https://localhost:7240

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
  <page name="Tenants" path="app/tenancy/tenants" description="Page to manage tenants" src="pages/tenants.ts">
    <components>
      <component name="Tenant List" type="datatable"/>
      <component name="Tenant List Paginator" type="paginator"/>
      <component name="Create Tenant Form" type="modal"/>
      <component name="Update Tenant Form" type="modal"/>
    </components>
  </page>
  <page name="Users" path="app/users" description="Page to manage users" src="pages/users.ts">
    <components>
      <component name="User List" type="datatable"/>
      <component name="User List Paginator" type="paginator"/>
      <component name="Create User Form" type="modal"/>
      <component name="Update User Form" type="modal"/>
    </components>
  </page>
  <page name="Roles" path="app/roles" description="Page to manage roles" src="pages/roles.ts">
    <components>
      <component name="Role List" type="datatable"/>
      <component name="Role List Paginator" type="paginator"/>
      <component name="Create Role Form" type="modal"/>
      <component name="Update Role Form" type="modal"/>
    </components>
  </page>
</pages>
```