---
name: fe.test.conventions
description: "Frontend unit test conventions for Angular projects using Vitest."
applyTo: "srcs/apps/Angular/**/*.spec.ts"
---

# INSTRUCTION - Frontend Unit Test Conventions

## Framework & Imports

- **Vitest only**: Import `describe, it, beforeEach, afterEach, beforeAll, afterAll, expect, vi` from `vitest`
- **Never** use Jest or Jasmine imports
- Mock with `vi.fn()` and `vi.mock()`, not `jest.fn()`

## Test Organization

- First test: Always verify creation with `'should create'`
- Group related tests logically
- Use descriptive test names starting with `'should'`
- Test one behavior per `it` block
- Use `src/testing/` helpers (e.g., `MockLoggerService`) to avoid console noise

## Naming & Structure

- File location: `*.spec.ts` next to source file
- Path aliases: Use `@components/`, `@shared/` for imports
- Suite naming: 
  - `describe('Category.ComponentName', () => {})`
  - `describe('Services.ServiceName', () => {})`
  - `describe('Pages.PageName', () => {})`

## Mocking Patterns

- **Shared mocks**: Define outside `describe` block when reused across tests
- **Shared mock classes**: Create in `src/testing/` and import as needed
- **External libraries**: Use `vi.mock('library-name', () => ({ ... }))`
- **Services**: Create mock objects with `vi.fn()` methods, provide via TestBed
- **Console**: Replace `globalThis.console` in `beforeAll`, restore in `afterAll`
- **Cleanup**: Always call `vi.clearAllMocks()` in `afterEach`

## Common Assertions

- Component creation: `expect(component).toBeTruthy()`
- Method calls: `expect(mockFn).toHaveBeenCalled()` or `.toHaveBeenCalledWith(args)`
- DOM content: `expect(fixture.nativeElement.textContent).toContain('text')`
- Private members: Access via `(instance as any).privateMember`

## Component Tests

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, expect, vi } from 'vitest';

describe('Components.ComponentName', () => {
  let component: ComponentName;
  let fixture: ComponentFixture<ComponentName>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentName], // or declarations for non-standalone
      providers: [
        { provide: ServiceName, useValue: mockService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ComponentName);
    component = fixture.componentInstance;
    // Set required inputs before detectChanges
    component.requiredInput = 'value';
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
```

## Service Tests

```typescript
import { TestBed } from '@angular/core/testing';
import { describe, it, beforeEach, afterEach, expect, vi } from 'vitest';

describe('Services.ServiceName', () => {
  let service: ServiceName;

  beforeEach(() => {
    vi.clearAllMocks();
    TestBed.configureTestingModule({
      providers: [ServiceName]
    });
    service = TestBed.inject(ServiceName);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });
});
```
