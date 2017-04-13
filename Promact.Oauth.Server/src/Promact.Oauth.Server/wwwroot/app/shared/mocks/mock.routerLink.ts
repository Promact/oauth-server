import { Input, Directive, HostBinding } from '@angular/core';
@Directive({
    selector: '[routerLink]',
})
export class RouterLinkStubDirective {
    @Input() linkParams: any;
    navigatedTo: any = null;
    @HostBinding('click') role = 'onClick()';

    onClick() {
        this.navigatedTo = this.linkParams;
    }
}

