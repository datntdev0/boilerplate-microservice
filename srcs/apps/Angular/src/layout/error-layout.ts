import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  imports: [RouterModule],
  selector: 'error-layout',
  templateUrl: './error-layout.html',
  host: { "class": "d-flex flex-column flex-root" }
})
export class ErrorLayout { }
