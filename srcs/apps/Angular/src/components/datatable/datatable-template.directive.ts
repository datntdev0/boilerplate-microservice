import { Directive, Input, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[appDatatableTemplate]',
  standalone: false
})
export class DatatableTemplateDirective {
  @Input() column!: string; // The column key

  constructor(public template: TemplateRef<any>) {}
}
