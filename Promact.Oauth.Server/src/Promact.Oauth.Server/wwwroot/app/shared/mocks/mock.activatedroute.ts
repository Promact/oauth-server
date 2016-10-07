//import { Injectable } from '@angular/core';
//import { Router, ActivatedRoute } from '@angular/router';
////mock Router service
//@Injectable()
//class MockActivatedRoute extends ActivatedRoute {
//    constructor() {
//        super()
//        this.params = Observable.of({ id: "5" });
//    }
//}
//import { Injectable } from '@angular/core';

//@Injectable()
//export class MockActivatedRoute  {
//    subscribe() { return true; }
//}

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Injectable } from '@angular/core';
@Injectable()
export class ActivatedRouteStub {

    // ActivatedRoute.params is Observable
    private subject = new BehaviorSubject(this.testParams);
    params = this.subject.asObservable();

    // Test parameters
    private _testParams: {};
    get testParams() { return this._testParams; }
    set testParams(params: {}) {
        this._testParams = params;
        this.subject.next(params);
    }

    // ActivatedRoute.snapshot.params
    get snapshot() {
        return { params: this.testParams };
    }
}
