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
    IsLoading: false,
    Players: [],
    SelectedPlayer: '',
    DateFrom: '',
    DateTo: ''
};

const dataReducer: Reducer<AppState> = (
    state: AppState = initialState,
    action: ActionTypes
): AppState => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                IsLoading: false,
                Players: action.payload.Players,
                DateFrom: action.payload.DateFrom,
                DateTo: action.payload.DateTo,
                SelectedPlayer: ''
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
        data: dataReducer
    });

export default createRootReducer;

