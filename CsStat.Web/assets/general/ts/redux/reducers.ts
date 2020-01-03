import { Reducer, combineReducers } from 'redux';
import { connectRouter } from 'connected-react-router';
import {
    AppState,
    ActionTypes,
    SELECT_PLAYER,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST
} from './types';

export const initialState: AppState = {
    data: {
        IsLoading: false,
        Players: [],
        SelectedPlayer: '',
        DateFrom: '',
        DateTo: ''
    }
};

const rootReducer: Reducer<AppState> = (
    state: AppState = initialState,
    action: ActionTypes
): AppState => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                data: {
                    IsLoading: false,
                    Players: action.payload.data.Players,
                    DateFrom: action.payload.data.DateFrom,
                    DateTo: action.payload.data.DateTo,
                    SelectedPlayer: ''
                }
            };
        case SELECT_PLAYER:
            return {
                ...state,
                data: {
                    ...state.data,
                    SelectedPlayer: action.payload
                }
            };

        case START_REQUEST:
            return {
                ...state,
                data: {
                    ...state.data,
                    IsLoading: true
                }
            };

        case STOP_REQUEST:
            return {
                ...state,
                data: {
                    ...state.data,
                    IsLoading: false
                }
            };

        default:
            return state;
    }
};

export const createRootReducer = (history: any): Reducer =>
    combineReducers({
        router: connectRouter(history),
        data: rootReducer
    });

export default createRootReducer;

