import { FETCH_PLAYERS_DATA } from '../redux-constants';


const initialState = {
    items: [],
    selected: {}
};

export default (state = initialState, action) => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                items: action.payload
            };
        default:
            return state;
    }
};
