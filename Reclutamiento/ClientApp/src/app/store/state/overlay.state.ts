import { State, Selector, Action, StateContext } from "@ngxs/store";
import { Overlay } from '../actions/overlay.actions';

export class OverlayStateModel {
    display: boolean;
}

@State<OverlayStateModel>({
    name: 'overlay',
    defaults: {
        display: false,
    }
})

export class OverlayState {

    @Selector()
    static getOverlayDisplay(state: OverlayStateModel) {
        return state.display;
    }

    @Action(Overlay.Toggle)
    set({ getState, setState }: StateContext<OverlayStateModel>) {
        const state = getState();
        setState({
            display: !state.display
        });
    }

}