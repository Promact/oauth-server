import {Component, Input} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import {Router, ActivatedRoute} from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
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
    listOfRoles: any;

    constructor(private userService: UserService, private redirectionRoute: Router, private route: ActivatedRoute, private toast: Md2Toast, private loader: LoaderService) {
        this.userModel = new UserModel();
        this.listOfRoles = [];
        this.isEmailExist = false;
    }


    ngOnInit() {
        this.getRoles();
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
        if (this.isSlackUserNameExist == true) {
            if (this.isEmailExist == false) {
                this.userService.registerUser(this.userModel).subscribe((result) => {
                    if (result == true) {
                        this.toast.show('User added successfully.');
                        this.redirectionRoute.navigate(['user/list']);
                    }
                    else if (result == false) {
                        this.toast.show('User Name already exists.');
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

        this.userService.findUserByEmail(email + "@promactinfo.com").subscribe((isEmailExist) => {
            if (isEmailExist) {
                this.isEmailExist = true;
            }
            else {
                this.isEmailExist = false;

            }
        }, err => {
        });
    }

    checkSlackUserName(slackUserName) {
        this.isSlackUserNameExist = false;
        this.userService.findUserBySlackUserName(slackUserName).subscribe((isSlackUserNameExist) => {
            if (isSlackUserNameExist) {
                this.isSlackUserNameExist = true;
            }
            else {
                this.isSlackUserNameExist = false;
            }
        }, err => {
        });
    }

    goBack() {
        this.redirectionRoute.navigate(['user/list']);
    }


}

