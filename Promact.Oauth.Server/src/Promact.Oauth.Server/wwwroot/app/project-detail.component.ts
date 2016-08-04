//import { Component, Input } from '@angular/core';

//@Component({
//    selector: 'my-project-detail',
//})
//export class ProjectDetailComponent {
//}


import { Component, Input } from '@angular/core';
import { Pro } from './Pro';
@Component({
    selector: 'my-pro-detail',
    template: `
    <div *ngIf="pro">
      <h2>{{pro.name}} details!</h2>
      <div><label>id: </label>{{pro.id}}</div>
      <div>
        <label>name: </label>
        <input [(ngModel)]="pro.name" placeholder="name"/>
      </div>
    </div>
  `
})
export class ProDetailComponent {
    @Input()
    pro: Pro;
}
