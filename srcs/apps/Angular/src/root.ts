import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({
  imports: [RouterOutlet],
  selector: 'root-component',
  template: '<router-outlet></router-outlet>',
  host: { "class": "d-flex flex-column flex-root" }
})
export default class RootComponent { }
