import { FETCH_PLAYERS_DATA, SELECT_PLAYER } from '../redux-constants';

const fetchPlayers = (playersDataUrl) => (dispatch) => {
    fetch(playersDataUrl)
        .then((res) => res.json())
        .then((players) => {
            players = typeof players === 'string' ? JSON.parse(players) : players;
            dispatch({
                type: FETCH_PLAYERS_DATA,
                payload: players
            });
        });
};

const selectPlayer = (playerId) => (dispatch) => {
    dispatch({
        type: SELECT_PLAYER,
        payload: playerId
    });
};

export { fetchPlayers, selectPlayer };
