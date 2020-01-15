import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'


@NgModule({
    providers: [],
    declarations: [
    ],
    exports: [
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
