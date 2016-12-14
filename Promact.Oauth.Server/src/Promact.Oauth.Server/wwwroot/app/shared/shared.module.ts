import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Md2Module } from 'md2';
import { MaterialModule } from '@angular/material';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        MaterialModule.forRoot(),
        Md2Module.forRoot()
    ],
    declarations: [],
    exports: [CommonModule, FormsModule,MaterialModule,Md2Module]
})
export class SharedModule { }