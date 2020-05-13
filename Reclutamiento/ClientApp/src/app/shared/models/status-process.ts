import { StateValidation } from '../enums/state-validation.enum';

export interface StatusProcess {
    id?: number;
    name: string;
    stateValidation: StateValidation;
    info?: string | null;
    userName?: string | null;
    date?: Date | null;
    description?: string | null;
    canApprove?: boolean | null;
    canDeny?: boolean | null;
    canCancel?: boolean | null;
    error?: boolean | null;
    index: number;
    currentIndex?: number;
    timelapse?: string;
    descriptionRequired?: boolean | null;
    visible?: boolean | null;
}
