import {Component} from "@angular/core";
import { Router, ROUTER_DIRECTIVES}from '@angular/router';


@Component({
    templateUrl: "app/consumerapp/consumerapp-list/consumerapp-list.html",
    directives: []
})
export class ConsumerappListComponent
{
    constructor(private router: Router) {

    }

    addNewApp() {
        this.router.navigate(['/consumerapp/add']);
    }
} 