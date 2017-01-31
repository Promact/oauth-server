import { UserModel } from '../users/user.model';
export class ProjectModel {
    id: number;
    name: string;
    slackChannelName: string;
    isActive: boolean;
    teamLeaderId: string;
    createdBy: string;
    createdDate: Date;
    createdOns: string;
    updatedBy: string;
    updatedDate: Date;
    updatedOns: string;
    uniqueName: string;
    teamLeader: UserModel;
    listUsers: Array<UserModel>;
    applicationUsers: Array<UserModel>;
}