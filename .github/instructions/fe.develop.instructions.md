---
name: fe.develop.conventions
description: "Use when developing Angular features, modules, components, services, guards, pipes, or any source file in the Angular frontend. Covers file naming, class naming, folder structure, routing, DI, state management, HTTP proxies, and shared patterns. Excludes unit test conventions."
applyTo: "srcs/apps/Angular/src/**/*.ts"
---

# INSTRUCTION - Frontend Development Conventions

## Project Overview

- **Angular 21** with `bootstrapApplication` (standalone bootstrap)
- **No Angular Material** — UI is Bootstrap 5 + ngx-bootstrap
- **No state management library** — use Angular signals and services
- **NSwag-generated proxies** for all HTTP/API communication
- **OIDC authentication** via `oidc-client-ts` wrapped in `AuthService`

---

## Folder Structure

```
src/
  main.ts                   # bootstrapApplication entry point
  main.config.ts            # ApplicationConfig with all root providers
  main.routes.ts            # Root route definitions
  root.ts                   # Root component
  root.initializer.ts       # APP_INITIALIZER + proxy provider factories
  components/               # Reusable UI components (NgModule-based)
  layout/                   # Shell layout components (standalone)
  modules/                  # Feature modules (lazy-loaded NgModules)
    <feature>/
      <feature>.module.ts
      pages/
        <page>.ts
        <page>.html
        <page>.spec.ts
  shared/
    guards/                 # Functional route guards
    models/                 # Shared model interfaces and constants
    pages/                  # Shared/error pages
    pipes/                  # Shared pipes
    proxies/                # NSwag-generated API proxy classes
    services/               # Root-provided application services
  styles/                   # Global SCSS
  testing/                  # Test utilities (excluded from production build)
```

---

## Naming Conventions

| Artifact | File name | Class name |
|---|---|---|
| Page | `<name>.ts` / `<name>.html` | `<Name>Page` |
| Component | `<name>.ts` / `<name>.html` | `<Name>Component` |
| Service | `<name>.service.ts` | `<Name>Service` |
| Layout | `<name>.ts` / `<name>.html` | `<Name>Layout` |
| Module | `<name>.module.ts` | `<Name>Module` |
| Guard | `<name>.guard.ts` | (functional, no class) |
| Pipe | `<name>.pipe.ts` | `<Name>Pipe` |
| Spec | `<name>.spec.ts` | — |

- **File names**: `kebab-case`; no `.component.ts` suffix — role is expressed in the class name; services use `.service.ts` (e.g., `auth.service.ts`), guards use `.guard.ts` (e.g., `auth.guard.ts`)
- **CSS selectors**: `app-` prefix for reusable components (e.g., `app-datatable`); no prefix for pages and layouts
- **Signals**: `camelCase` + `Signal` suffix (e.g., `datatableSignal`, `isLoadingSignal`)
- **Injection tokens**: `SCREAMING_SNAKE_CASE` (e.g., `API_BASE_URL_ADMIN`)
- **Provider factories**: `provide` prefix (e.g., `provideSrvAdminProxy()`)

---

## Component & Page Patterns

### Standalone vs NgModule

| Layer | Pattern |
|---|---|
| Root / bootstrap | Standalone (`bootstrapApplication`) |
| Layout shell (`MainLayout`, `ErrorLayout`) | Standalone component |
| Header, Sidebar, Toast | Standalone components |
| Feature pages (in `src/modules/`) | `standalone: false` — declared in their feature `NgModule` |
| Reusable UI (`src/components/`) | `standalone: false` — declared in `ComponentsModule` |

### Dependency Injection

- **Prefer `inject()`** over constructor injection in pages and standalone components:

```ts
export class TenantsPage implements OnInit {
  private readonly clientAdminSrv = inject(SrvAdminClientProxy);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);
}
```

- Use constructor injection only when the class is an `NgModule`-declared component/service that requires older DI style.

### Reactive State with Signals

- Use `signal()` for component-local reactive state:

```ts
public datatableSignal = signal(new Datatable<TenantListDto>());
public isLoadingSignal = signal(false);
```

- Expose signals as `public` for template binding; keep them `readonly` where mutation should be internal only.
- Use `computed()` for derived values, `effect()` sparingly and only when side effects are unavoidable.

### Forms

- Always use **Reactive Forms** (`FormBuilder`, `FormGroup`, `FormControl`).
- Never use Template-driven forms.

---

## Services

- Declare services with `@Injectable({ providedIn: 'root' })` for tree-shakable singleton registration.
- Services manage shared state via `BehaviorSubject` (exposed as `Observable`) or `signal()`.
- Place application-wide services under `src/shared/services/`.
- Feature-scoped services (rare) can be declared in the feature module's `providers` array.

```ts
@Injectable({ providedIn: 'root' })
export class ToastService {
  private readonly toasts = signal<ToastConfig[]>([]);
  public readonly toasts$ = toObservable(this.toasts);
}
```

---

## Feature Module Structure

Each feature module under `src/modules/<feature>/` must follow this shape:

```ts
// <feature>.module.ts
@NgModule({
  declarations: [FeaturePage],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    ComponentsModule,
  ],
  providers: [provideSrvAdminProxy()],  // add required proxy providers
})
export class FeatureModule {}

const routes: Routes = [
  { path: '', component: FeaturePage },
];
```

- Always use `RouterModule.forChild(routes)` inside feature modules.
- Feature modules are **never** eagerly imported — they are always lazy-loaded from `main.routes.ts`.

---

## Routing

### Root Routes (`main.routes.ts`)

- Protected routes go under the `MainLayout` shell with `canActivate: [authGuard]`.
- Lazy-load each feature module using `loadChildren`:

```ts
{
  path: 'app',
  component: MainLayout,
  canActivate: [authGuard],
  children: [
    {
      path: 'feature',
      loadChildren: () =>
        import('./modules/feature/feature.module').then(m => m.FeatureModule),
    },
  ],
},
```

- Error pages go under `ErrorLayout` at `path: 'error'`.
- Always add a wildcard redirect to `/error/404` as the last route.

### Route Guards

- Guards are **functional** (`CanActivateFn`), never class-based:

```ts
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  // ...
};
```

- Place guards in `src/shared/guards/`.

---

## HTTP / API Communication

### NSwag Proxies

- **Never** use `HttpClient` directly in pages or services. Use the NSwag-generated proxy classes from `src/shared/proxies/`.
- Each backend service has its own proxy file: `srv-<service>-proxies.ts`.
- Proxy classes are `@Injectable()` without `providedIn: 'root'` — they must be provided explicitly.

### Providing Proxies

- Proxy providers are registered via factory functions in `root.initializer.ts`:

```ts
export function provideSrvAdminProxy(): Provider[] {
  return [
    SrvAdminClientProxy,
    { provide: API_BASE_URL_ADMIN, useValue: `${environment.apiUrl}/srv-admin` },
  ];
}
```

- Include the relevant `provide<Service>Proxy()` in the feature module's `providers` array.
- New backend services require a new `InjectionToken` for their base URL and a corresponding factory function in `root.initializer.ts`.

### Error Handling

- All API errors are handled globally by `GlobalErrorHandler` (registered in `main.config.ts`).
- Do not add per-component `try/catch` for HTTP errors — let `GlobalErrorHandler` route them to `ToastService`.

---

## Shared Utilities

### Path Aliases

Always use path aliases for cross-folder imports:

| Alias | Resolves to |
|---|---|
| `@shared` | `src/shared` |
| `@shared/*` | `src/shared/*` |
| `@components` | `src/components` |
| `@components/*` | `src/components/*` |

```ts
import { AuthService } from '@shared/services/auth-service';
import { DatatableComponent } from '@components/datatable/datatable';
```

### Environment

- Import environment from `envs/environment` (not `src/environments/`):

```ts
import { environment } from 'envs/environment';
```

- The `envs/` folder is replaced at build time — never hard-code environment-specific values in source files.

### ComponentsModule

- Import `ComponentsModule` in any feature module that needs `DatatableComponent`, `PaginatorComponent`, or `DialogComponent`.
- Do not re-declare components that are already in `ComponentsModule`.

### Pipes

- Place shared pipes under `src/shared/pipes/`.
- Pipes must be standalone and declared with `@Pipe({ standalone: true })`.

---

## Storybook

- Component stories live in `stories/` at the project root.
- Write stories for every reusable component added to `ComponentsModule`.
- Use `@storybook/angular` CSF3 format.
