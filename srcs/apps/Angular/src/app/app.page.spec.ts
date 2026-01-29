import { TestBed } from '@angular/core/testing';
import { AppPage } from './app.page';

describe('Pages.App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppPage],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppPage);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render hello title', async () => {
    const fixture = TestBed.createComponent(AppPage);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, datntdev');
    expect(compiled.querySelector('h2')?.textContent).toContain('This is Microservice.App.Angular');
  });
});
