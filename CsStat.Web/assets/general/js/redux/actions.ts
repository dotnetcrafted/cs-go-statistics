import { AppState, ActionTypes, SELECT_PLAYER, FETCH_PLAYERS_DATA, START_REQUEST } from './types';

const fetchPlayers = (newState: AppState): ActionTypes => ({
    type: FETCH_PLAYERS_DATA,
    payload: newState
})

const startRequest = (): ActionTypes => ({
    type: START_REQUEST
})

const selectPlayer = (playerId: string): ActionTypes => ({
    type: SELECT_PLAYER,
    payload: playerId
})

export { fetchPlayers, startRequest, selectPlayer };
