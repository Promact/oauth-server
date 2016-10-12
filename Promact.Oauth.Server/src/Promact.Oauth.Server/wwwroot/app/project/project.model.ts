import { UserModel } from '../users/user.model'
export class projectModel {
    id: number;
    name: string;
    slackChannelName: string;
    isActive: boolean;
    teamLeaderId: string;
    createdBy: string;
    createdDate: string;
    updatedBy: string;
    updatedDate: string;
    uniqueName: string;
    teamLeader: UserModel;
    listUsers: Array<UserModel>;
    applicationUsers: Array<UserModel>;
}
