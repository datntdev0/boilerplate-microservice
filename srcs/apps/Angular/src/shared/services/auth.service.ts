import { Injectable, signal } from '@angular/core';
import { AUTH_CONFIGS } from '@shared/models/constants';
import { SigninRedirectArgs, SignoutRedirectArgs, User, UserManager } from 'oidc-client-ts';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private userManager = new UserManager(AUTH_CONFIGS);
  
  public userSignal = signal<User | null>(null);

  constructor() { this.setupEventListeners(); }

  private setupEventListeners(): void {
    this.userManager.events.addUserLoaded((user: User) => {
      this.userSignal.set(user);
    });
    this.userManager.events.addUserUnloaded(() => {
      this.userSignal.set(null);
    });
    this.userManager.events.addAccessTokenExpired(() => {
      console.log('Access token expired');
    });
    this.userManager.events.addUserSignedOut(() => {
      console.log('User signed out');
    });
  }

  public async initialize(initialUrl: string): Promise<void> {
    if (initialUrl.startsWith('/error/')) return;

    await this.userManager.clearStaleState();
    await this.userManager.metadataService.getMetadata();

    const user = await this.userManager.getUser();
    if (!(user?.expired ?? true)) this.userSignal.set(user);
  }

  public signIn(args?: SigninRedirectArgs): Promise<void> {
    return this.userManager.signinRedirect(args);
  }

  public signInCallback(): Promise<User> {
    return this.userManager.signinRedirectCallback();
  }

  public signOut(args?: SignoutRedirectArgs): Promise<void> {
    return this.userManager.signoutRedirect(args);
  }
}
