const fetchPlayersr = (playersDataUrl, params) => (dispatch) => {
    const url = new URL(playersDataUrl, window.location.origin);
    if (params) {
        url.search = new URLSearchParams(params);
    }

    dispatch({
        type: START_REQUEST
    });

    fetch(url)
        .then((res) => res.json())
        .then((data) => {
            data = typeof data === 'string' ? JSON.parse(data) : data;
            dispatch({
                type: FETCH_PLAYERS_DATA,
                payload: data
            });
        });
};

const selectPlayerr = (playerId) => (dispatch) => {
    dispatch({
        type: SELECT_PLAYER,
        payload: playerId
    });
};
