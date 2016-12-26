import { UserModel } from '../users/user.model';
export class ProjectModel {
    Id: number;
    Name: string;
    SlackChannelName: string;
    IsActive: boolean;
    TeamLeaderId: string;
    CreatedBy: string;
    CreatedDate: string;
    CreatedOns: string;
    UpdatedBy: string;
    UpdatedDate: string;
    UpdatedOns: string;
    UniqueName: string;
    TeamLeader: UserModel;
    ListUsers: Array<UserModel>;
    ApplicationUsers: Array<UserModel>;
}
