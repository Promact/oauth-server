﻿import {UserModel} from '../users/user.model'
export class projectModel {
    Id: number;
    Name: string;
    SlackChannelName: string;
    IsActive: boolean;
    TeamLeaderId: string;
    CreatedBy: string;
    CreatedDate: string;
    UpdatedBy: string;
    UpdatedDate: string;
    UniqueName: String;
    TeamLeader: UserModel;
    ListUsers: Array<UserModel>;
    ApplicationUsers: Array<UserModel>;
} 