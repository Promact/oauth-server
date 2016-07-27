import { Injectable } from '@angular/core';

import 'rxjs/add/operator/toPromise';
import { Pro } from './Pro';
import { PROS } from './mock-project';
@Injectable()
export class ProService {
    private ProjectUrl = 'app/project';  // URL to web api
    
    getPros() {
        return Promise.resolve(PROS);
    }
    

}
