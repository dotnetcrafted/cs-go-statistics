import {
    IAppState,
    Post,
    ActionTypes,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_POSTS_DATA
} from './types';

const fetchPlayers = (newState: IAppState): ActionTypes => ({
    type: FETCH_PLAYERS_DATA,
    payload: newState
});

const fetchPosts = (posts: Post[]): ActionTypes => ({
    type: FETCH_POSTS_DATA,
    payload: posts
});

const startRequest = (): ActionTypes => ({
    type: START_REQUEST
});

const stopRequest = (): ActionTypes => ({
    type: STOP_REQUEST
});

export {
    fetchPlayers, startRequest, stopRequest, fetchPosts
};
