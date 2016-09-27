import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MdButtonModule } from "@angular2-material/button";
import { MdToolbarModule } from "@angular2-material/toolbar";
import { MdInputModule } from "@angular2-material/input";
import { MdSidenavModule } from "@angular2-material/sidenav";
import { MdCheckboxModule } from "@angular2-material/checkbox";
import { Md2Module } from "md2/all";
//import { MdSelect, MdOption, MdSelectDispatcher } from "../select/select";
//import { MdMultiselect } from '../multiselect/multiselect';

@NgModule({
    imports: [
        CommonModule,
        MdButtonModule.forRoot(),
        MdToolbarModule.forRoot(),
        MdSidenavModule.forRoot(),
        MdInputModule.forRoot(),
        MdCheckboxModule.forRoot(),
        Md2Module.forRoot()],
    exports: [CommonModule, FormsModule, MdButtonModule, MdToolbarModule, MdInputModule, MdCheckboxModule, Md2Module]
})
export class SharedModule { }