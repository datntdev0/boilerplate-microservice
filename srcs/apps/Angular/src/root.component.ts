import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({
  selector: 'root-component',
  template: '<router-outlet></router-outlet>',
  imports: [RouterOutlet]
})
export default class RootComponent { }
