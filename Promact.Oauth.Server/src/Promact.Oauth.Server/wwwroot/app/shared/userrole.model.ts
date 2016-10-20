declare var user: {userId:string, userName: string, name: string, role: string };
export class UserRole {
    public Role: string;
    public Id: string;
    constructor() {
        this.Role = user.role;
        this.Id = user.userId;
    }
}