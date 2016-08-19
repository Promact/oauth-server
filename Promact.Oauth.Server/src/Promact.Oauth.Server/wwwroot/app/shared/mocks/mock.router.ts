import { Injectable } from '@angular/core';

//mock Router service
@Injectable()
export class MockRouter {
    navigate() {
        return true;
    }
}