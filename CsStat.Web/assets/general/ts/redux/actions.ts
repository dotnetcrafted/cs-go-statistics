import {
    IAppState,
    Post,
    ActionTypes,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_POSTS_DATA,
    FILTER_BY_TAG,
    REFRESH_POSTS
} from "./types";

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

const filteredByTag = (
    tag: string,
    tagsArr: string[],
    posts: Post[]
): ActionTypes => ({
    type: FILTER_BY_TAG,
    tag,
    tagsArr,
    payload: posts
});

const refreshPosts = (posts: Post[]): ActionTypes => ({
    type: REFRESH_POSTS,
    payload: posts
});

export {
    fetchPlayers,
    startRequest,
    stopRequest,
    fetchPosts,
    filteredByTag,
    refreshPosts
};
