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
    isLoading: false,
    players: [],
    posts: []
};

const appReducer: Reducer<IAppState> = (
    state: IAppState = initialState,
    action: ActionTypes
): IAppState => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                isLoading: false,
                players: action.payload.players
            };
        case FETCH_POSTS_DATA:
            return {
                ...state,
                isLoading: false,
                posts: action.payload,
            };

        case START_REQUEST:
            return {
                ...state,
                isLoading: true
            };

        case STOP_REQUEST:
            return {
                ...state,
                isLoading: false
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

