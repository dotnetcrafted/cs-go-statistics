import { FETCH_PLAYERS_DATA, SELECT_PLAYER, START_REQUEST } from '../redux-constants';


const initialState = {
    isLoading: true,
    allPlayers: [],
    selectedPlayer: '',
    DateFrom: '',
    DateTo: ''
};

export default (state = initialState, action) => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                isLoading: false,
                allPlayers: action.payload.Players,
                DateFrom: action.payload.DateFrom,
                DateTo: action.payload.DateTo
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
