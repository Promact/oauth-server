import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Md2Module } from 'md2';
import {
    MdSidenavModule,
    MdInputModule
} from '@angular/material';


@NgModule({
    imports: [
        CommonModule,
        MdSidenavModule,
        MdInputModule
    ],
    exports: [CommonModule, FormsModule, Md2Module],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule { }