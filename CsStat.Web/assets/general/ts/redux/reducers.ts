import { Reducer, combineReducers } from 'redux';
import { connectRouter } from 'connected-react-router';
import {
    IAppState,
    ActionTypes,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_POSTS_DATA
} from './types';

export const initialState: IAppState = {
    IsLoading: false,
    players: [],
    Posts: []
};

const appReducer: Reducer<IAppState> = (
    state: IAppState = initialState,
    action: ActionTypes
): IAppState => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                IsLoading: false,
                players: action.payload.players
            };
        case FETCH_POSTS_DATA:
            return {
                ...state,
                IsLoading: false,
                Posts: action.payload,
            };

        case START_REQUEST:
            return {
                ...state,
                IsLoading: true
            };

        case STOP_REQUEST:
            return {
                ...state,
                IsLoading: false
            };

        default:
            return state;
    }
};

export const createRootReducer = (history: any): Reducer =>
    combineReducers({
        router: connectRouter(history),
        app: appReducer
    });

export default createRootReducer;

