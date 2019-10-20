export interface IManagedMessage {
    id: string;

    channelId: string;
    moderatorRoleId: string;

    lastModified: Date;
    content: string;
}
