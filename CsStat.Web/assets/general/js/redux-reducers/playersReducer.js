import { FETCH_PLAYERS_DATA, SELECT_PLAYER, START_REQUEST } from '../redux-constants';


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

        case START_REQUEST:
            return {
                ...state,
                isLoading: true
            };
        default:
            return state;
    }
};
