import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({
  imports: [RouterOutlet],
  selector: 'root-component',
  template: '<router-outlet></router-outlet>',
})
export default class RootComponent { }
