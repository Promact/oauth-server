export class ConsumerAppModel {
        Name: string;
        CallbackUrl: string;
        AuthId: string;
        AuthSecret: string;
        Scopes: Array<consumerappallowedscopes>;
        LogoutUrl: string;
        Id: number;
}

export enum consumerappallowedscopes {
    email,
    openid,
    profile,
    user_read,
    project_read
}

      