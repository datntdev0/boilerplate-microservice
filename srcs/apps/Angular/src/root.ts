import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { ToastComponent } from "@components/toast/toast";

@Component({
  imports: [RouterOutlet, ToastComponent],
  selector: 'root-component',
  template: '<router-outlet/><app-toast/>',
  host: { "class": "d-flex flex-column flex-root" }
})
export default class RootComponent { }
