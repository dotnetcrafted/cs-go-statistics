import { FETCH_PLAYERS_DATA, SELECT_PLAYER, START_REQUEST } from '../redux-constants';

const fetchPlayers = (playersDataUrl, params) => (dispatch) => {
    const url = new URL(playersDataUrl, window.location.origin);
    if (params) {
        url.search = new URLSearchParams(params);
    }

    dispatch({
        type: START_REQUEST
    });

    fetch(url)
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
