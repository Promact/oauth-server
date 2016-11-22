﻿import { Component, Input } from "@angular/core";
import { UserService } from '../user.service';
import { UserModel } from '../user.model';
import { SlackUserModel } from '../slackUser.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';



@Component({
    templateUrl: 'app/users/user-add/user-add.html'

})

export class UserAddComponent {
    isEmailExist: boolean;
    isUserNameExist: boolean;
    isSlackUserNameExist: boolean;
    @Input()
    userModel: UserModel;
    slackUserModel: SlackUserModel;
    listOfRoles: any;
    listOfSlackUser: any;

    constructor(private userService: UserService, private redirectionRoute: Router, private route: ActivatedRoute, private toast: Md2Toast, private loader: LoaderService) {
        this.userModel = new UserModel();
        this.slackUserModel = new SlackUserModel();
        this.listOfRoles = [];
        this.listOfSlackUser = [];
        this.isEmailExist = false;
        this.isSlackUserNameExist = false;
    }


    ngOnInit() {
        this.getRoles();
        this.fetchSlackUserDetails();
    }

    fetchSlackUserDetails() {
        this.userService.fetchSlackUserDetails().subscribe((result) => {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    this.listOfSlackUser.push(result[i]);
                }
            }
        }, err => {
            console.log(err);
        });
    }

    getRoles() {
        this.userService.getRoles().subscribe((result) => {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    this.listOfRoles.push(result[i]);
                }
            }
        }, err => {
        });
    }

    addUser(userModel) {
        this.loader.loader = true;
        userModel.JoiningDate = new Date(userModel.JoiningDate);
        if (!this.isSlackUserNameExist) {
            if (!this.isEmailExist) {
                userModel.FirstName = userModel.FirstName.trim();
                this.userService.registerUser(userModel).subscribe((result) => {
                    if (result) {
                        this.toast.show('User added successfully.');
                        this.redirectionRoute.navigate(['user/list']);
                    }
                    else if (!result) {
                        this.toast.show('Email is invalid.');
                    }
                    this.loader.loader = false;
                }, err => {
                });
            }
            else {
                this.loader.loader = false;
                this.toast.show('Email Address already exists.');
            }
        }
        else {
            this.loader.loader = false;
            this.toast.show('Slack User Name  already exists.');
        }
    }

    checkEmail(email) {
        this.isEmailExist = false;
        if (email !== "" && email !== undefined) {
            this.userService.checkEmailIsExists(email).subscribe((result) => {
                this.isEmailExist = result;
            }, err => {
                console.log(err);
            });
        }
    }

    checkSlackUserId(slackUserId) {
        this.isSlackUserNameExist = false;
        if (slackUserId !== "" && slackUserId !== undefined) {
            this.userService.checkUserIsExistsBySlackUserId(slackUserId).subscribe((result) => {
                this.isSlackUserNameExist = result;
            }, err => {
                console.log(err.statusText);
            });
        }
    }

    goBack() {
        this.redirectionRoute.navigate(['user/list']);
    }


}

