import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

export function pristineOrminLength(length: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        var pristine = control.pristine;
        var hasLength = control.value.length >= length;
        if (pristine || hasLength){
            return null;
        }

        return { 'pristineOrMinLength': true };
    }
}