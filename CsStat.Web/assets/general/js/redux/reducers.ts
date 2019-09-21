import {Reducer} from 'redux';
import { IAppState, ActionTypes, SELECT_PLAYER, FETCH_PLAYERS_DATA, START_REQUEST } from './types';

const rootReducer: Reducer<IAppState>  = (state: IAppState = initialState, action: ActionTypes): IAppState => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                IsLoading: false,
                Players: action.payload.Players,
                DateFrom: action.payload.DateFrom,
                DateTo: action.payload.DateTo
            };
        case SELECT_PLAYER:
            return {
                ...state,
                SelectedPlayer: action.payload
            };

        case START_REQUEST:
            return {
                ...state,
                IsLoading: true
            };
        default:
            return state;
    }
};

const initialState: IAppState = {
    IsLoading: false,
    Players: [],
    SelectedPlayer: '',
    DateFrom: '',
    DateTo: ''
}

export default rootReducer;