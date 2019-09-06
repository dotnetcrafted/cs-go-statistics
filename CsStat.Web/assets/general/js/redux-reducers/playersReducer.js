import { FETCH_PLAYERS_DATA, SELECT_PLAYER } from '../redux-constants';


const initialState = {
    isLoading: true,
    allPlayers: [],
    selectedPlayer: ''
};

export default (state = initialState, action) => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                isLoading: false,
                allPlayers: action.payload
            };
        case SELECT_PLAYER:
            return {
                ...state,
                selectedPlayer: action.payload
            };
        default:
            return state;
    }
};
