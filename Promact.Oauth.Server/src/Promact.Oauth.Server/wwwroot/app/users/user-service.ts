import {Component, Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';

@Injectable()
export class UserService {

    constructor(private http: Http) { }

    getUsers()
    {
        return this.http.get('app/All');
    }
}