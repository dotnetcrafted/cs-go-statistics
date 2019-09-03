import { FETCH_PLAYERS_DATA } from '../redux-constants';

const fetchPlayers = () => (dispatch) => {
    fetch('api/playersdata')
        .then((res) => res.json())
        .then((players) =>
            dispatch({
                type: FETCH_PLAYERS_DATA,
                payload: players
            }));
};

export { fetchPlayers };
