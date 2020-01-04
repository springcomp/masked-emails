import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'

import { AngularSwitcheryComponent } from './_switchery/angular-switchery.component';
import { ModalComponent } from './_modal/modal.component'

@NgModule({
    providers: [],
    declarations: [
        AngularSwitcheryComponent,
        ModalComponent
    ],
    exports: [
        AngularSwitcheryComponent,
        ModalComponent
    ],
    imports: [
        CommonModule
    ]
})
export class CoreModule {
    static forRoot(){
        return {
            ngModule: CoreModule,
            providers: []
        };
    }
}