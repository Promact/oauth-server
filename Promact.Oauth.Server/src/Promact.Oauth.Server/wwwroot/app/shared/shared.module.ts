import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { Md2Module } from 'md2';


@NgModule({
    imports: [
        CommonModule,
        MaterialModule.forRoot(),
        Md2Module.forRoot()
    ],
    exports: [CommonModule, FormsModule, MaterialModule, Md2Module]
})
export class SharedModule { }