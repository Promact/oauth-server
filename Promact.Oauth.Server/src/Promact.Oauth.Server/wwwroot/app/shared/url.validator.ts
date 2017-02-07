import { Directive, forwardRef } from '@angular/core';
import { FormControl, NG_VALIDATORS } from '@angular/forms';


/**
 * function used to validate url.
 */
export function validateURLFactory() {
    return (c: FormControl) => {
        let URL_REGEX = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/;

        return URL_REGEX.test(c.value) ? null : { url: true };
    };
}

@Directive({
    selector: '[validateUrl][ngModel],[validateUrl][formControl]',
    providers: [
        { provide: NG_VALIDATORS, useClass: URLValidatorDirective, multi: true }
    ]
})
export class URLValidatorDirective {

    validator: Function;

    constructor() {
        this.validator = validateURLFactory();
    }

    validate(c: FormControl) {
        return this.validator(c);
    }
}