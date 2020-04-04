import { Reducer, combineReducers } from "redux";
import { connectRouter } from "connected-react-router";
import {
    IAppState,
    ActionTypes,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_POSTS_DATA,
    FILTER_BY_TAG,
    REFRESH_POSTS
} from "./types";

export const initialState: IAppState = {
    isLoading: false,
    players: [],
    posts: [],
    filteredPosts: []
};

const appReducer: Reducer<IAppState> = (
    state: IAppState = initialState,
    action: ActionTypes
): IAppState => {
    switch (action.type) {
        case FETCH_PLAYERS_DATA:
            return {
                ...state,
                isLoading: false,
                players: action.payload.players
            };
        case FETCH_POSTS_DATA:
            return {
                ...state,
                isLoading: false,
                posts: action.payload,
                filteredPosts: action.payload
            };

        case START_REQUEST:
            return {
                ...state,
                isLoading: true
            };

        case STOP_REQUEST:
            return {
                ...state,
                isLoading: false
            };
        case FILTER_BY_TAG:
            return {
                ...state,
                filteredPosts: state.filteredPosts.filter(post => {
                    const filter = post.tags.filter(
                        tag => tag.caption === action.tag
                    );
                    if (filter.length > 0) {
                        return post;
                    }
                })
            };
        case REFRESH_POSTS:
            return {
                ...state,
                filteredPosts: action.payload
            };
        default:
            return state;
    }
};

// export const initialFilterState: IFilterByTag = {
//     FilteredPosts: initialState.Posts
// };

// const filteredByTag: Reducer<IFilterByTag> = (
//     state: IFilterByTag = initialFilterState,
//     action: ActionTypes
// ): IFilterByTag => {
//     switch (action.type) {
//         case FILTER_BY_TAG:
//             return {
//                 ...state,
//                 FilteredPosts: []
//             };
//         default:
//             return state;
//     }
// };

export const createRootReducer = (history: any): Reducer =>
    combineReducers({
        router: connectRouter(history),
        app: appReducer
        // filter: filteredByTag
    });

export default createRootReducer;
