import {UserModel} from '../users/user.model'
export class projectModel {
    Id: number;
    Name: string;
    SlackChannelName: string;
    IsActive: boolean;
    TeamLeaderId: string;
    TeamLeader: UserModel;
    listUsers: Array<UserModel>;
    applicatioUsers: Array<UserModel>;
} 