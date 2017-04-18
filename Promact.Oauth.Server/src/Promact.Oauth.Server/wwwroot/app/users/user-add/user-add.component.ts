import { Component, Input, OnInit } from "@angular/core";
import { UserService } from '../user.service';
import { UserModel } from '../user.model';
import { UserRoleModel } from '../userrole.model';
import { Router } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { DatePipe } from '@angular/common';


@Component({
    moduleId: module.id,
    templateUrl: 'user-add.html'

})

export class UserAddComponent implements OnInit {
    isEmailExist: boolean;
    isUserNameExist: boolean;
    isSlackUserNameExist: boolean;
    @Input()
    userModel: UserModel;
    listOfRoles: Array<UserRoleModel>;
    constructor(private userService: UserService, private redirectionRoute: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.userModel = new UserModel();
        this.listOfRoles = new Array<UserRoleModel>();
        this.isEmailExist = false;
        this.isSlackUserNameExist = false;
    }


    ngOnInit() {
        this.getRoles();

    }

    getRoles() {
        this.userService.getRoles().then((result) => {
            if (result !== null) {
                for (let i = 0; i < result.length; i++) {
                    this.listOfRoles.push(result[i]);
                }
            }
        });
    }

    addUser(userModel: UserModel) {
        this.loader.loader = true;
        userModel.JoiningDate = new Date(userModel.JoiningDate);
        let datePipeMinDate = new DatePipe("medium");
        userModel.JoinDate = datePipeMinDate.transform(userModel.JoiningDate, "dd/MM/yyyy");
        
            if (!this.isEmailExist) {
                userModel.FirstName = userModel.FirstName.trim();
                this.userService.registerUser(userModel).then((result) => {
                    this.toast.show('User added successfully.');
                    this.redirectionRoute.navigate(['user/list']);
                    this.loader.loader = false;
                }, err => {
                    if (err.status === 400) {
                        this.toast.show('Email is invalid.');
                    } this.loader.loader = false;
                });
            }
            else {
                this.loader.loader = false;
                this.toast.show('Email already exists.');
            }
    }

    checkEmail(email: string) {
        this.isEmailExist = false;
        if (email !== "" && email !== undefined) {
            this.userService.checkEmailIsExists(email).then((result) => {
                this.isEmailExist = result;
            });
        }
    }

    goBack() {
        this.redirectionRoute.navigate(['user/list']);
    }


}

