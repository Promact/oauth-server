export class ConsumerAppModel {
        Name: string;
        CallbackUrl: string;
        AuthId: string;
        AuthSecret: string;
        Scopes: Array<consumerappallowedscopes>;
        LogoutUrl: string;
}

export enum consumerappallowedscopes {
    email,
    openid,
    profile,
    slack_user_id,
    user_read,
    project_read
}

      