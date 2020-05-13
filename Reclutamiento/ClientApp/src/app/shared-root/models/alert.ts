import { MessageType } from '../enums/message-type.enum';

export interface Alert {
    type: MessageType;
    title: string;
    description: string;
}
