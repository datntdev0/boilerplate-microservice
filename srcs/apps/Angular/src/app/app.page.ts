import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-page',
  imports: [RouterOutlet],
  templateUrl: './app.page.html',
  styleUrl: './app.page.scss'
})
export class AppPage {
  protected readonly person = signal('datntdev');
  protected readonly title = signal('Microservice.App.Angular');
}
