import { State, Selector, Action, StateContext } from "@ngxs/store";
import { Loader } from '../actions/loader.actions';

export class LoaderStateModel {
    display: boolean;
}

@State<LoaderStateModel>({
    name: 'loader',
    defaults: {
        display: false,
    }
})

export class LoaderState {

    @Selector()
    static getLoaderDisplay(state: LoaderStateModel) {
        return state.display;
    }

    @Action(Loader.Toggle)
    set({ getState, setState }: StateContext<LoaderStateModel>) {
        const state = getState();
        setState({
            display: !state.display
        });
    }

}
