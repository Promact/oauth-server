import { UserModel } from '../users/user.model'
export class projectModel {
    Id: number;
    name: string;
    SlackChannelName: string;
    IsActive: boolean;
    teamLeaderId: string;
    CreatedBy: string;
    CreatedDate: string;
    UpdatedBy: string;
    UpdatedDate: string;
    UniqueName: String;
    teamLeader: UserModel;
    listUsers: Array<UserModel>;
    applicationUsers: Array<UserModel>;
}
