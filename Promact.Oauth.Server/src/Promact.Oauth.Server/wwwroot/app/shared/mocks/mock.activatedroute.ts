//import { Injectable } from '@angular/core';
//import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
////mock Router service
//@Injectable()
//class MockActivatedRoute extends ActivatedRoute {
//    constructor() {
//        super()
//        this.params = Observable.of({ id: "5" });
//    }
//}
import { Injectable } from '@angular/core';

@Injectable()
export class MockActivatedRoute  {
    subscribe() { return true; }
}