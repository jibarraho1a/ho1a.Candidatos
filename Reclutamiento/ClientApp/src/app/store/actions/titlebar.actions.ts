import { TitleBar } from 'src/app/shared/models/title-bar';

export namespace titleBar {
    export class Add {
        static readonly type = '[Title Bar] Add';
        constructor(public payload: TitleBar) { }
    }

    export class Remove {
        static readonly type = '[Title Bar] Remove';
        constructor() { }
    }
}
