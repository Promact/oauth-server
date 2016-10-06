import { Injectable } from "@angular/core";

@Injectable()
export class MyService {
    private myValue;

    constructor() { }

    setValue(val) {
        this.myValue = val;
    }

    getValue() {
        return this.myValue;
    }
}