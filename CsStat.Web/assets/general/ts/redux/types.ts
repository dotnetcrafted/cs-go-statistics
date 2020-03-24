import { RouterState } from "connected-react-router";

interface IAppState {
    IsLoading: boolean;
    Players: Player[];
    Posts: Post[];
    FilteredPosts: Post[];
}

interface IFilterByTag {
    FilteredPosts: Post[];
}

type RootState = {
    router?: RouterState;
    app: IAppState;
    filter: IFilterByTag;
};

type Player = {
    Id: string;
    Name: string;
    ImagePath: string;
    Points: number;
    Kills: number;
    Deaths: number;
    Assists: number;
    FriendlyKills: number;
    KillsPerGame: number;
    AssistsPerGame: number;
    DeathsPerGame: number;
    DefusedBombs: number;
    ExplodedBombs: number;
    TotalGames: number;
    HeadShot: number;
    KdRatio: number;
    Achievements: Achievement[];
    Victims: RelatedPlayer[];
    Killers: RelatedPlayer[];
    Guns: Gun[];
};

type Achievement = {
    AchievementId: number;
    Name: string;
    Description: string;
    IconUrl: string;
};

type Gun = {
    Id: number;
    Name: string;
    Kills: number;
};

type RelatedPlayer = {
    Name: string;
    Count: number;
    ImagePath: string;
};

type Post = {
    Title: string;
    Content: string;
    tags: Tag[];
    createdAt: string;
    updatedAt: string;
};

type Tag = {
    Caption: string;
};

const FETCH_PLAYERS_DATA = "FETCH_PLAYERS_DATA";
const START_REQUEST = "START_REQUEST";
const STOP_REQUEST = "STOP_REQUEST";
const FETCH_POSTS_DATA = "FETCH_POSTS_DATA";
const FILTER_BY_TAG = "FILTER_BY_TAG";
const REFRESH_POSTS = "REFRESH_POSTS";

type FilterByTagAction = {
    type: typeof FILTER_BY_TAG;
    tag: string;
    payload: Post[];
};

type ResfreshPostsAction = {
    type: typeof REFRESH_POSTS;
    payload: Post[];
};

type StartRequestAction = {
    type: typeof START_REQUEST;
};

type StopRequestAction = {
    type: typeof STOP_REQUEST;
};

type FetchPlayersAction = {
    type: typeof FETCH_PLAYERS_DATA;
    payload: IAppState;
};

type FetchPostsAction = {
    type: typeof FETCH_POSTS_DATA;
    payload: Post[];
};

type ActionTypes =
    | StartRequestAction
    | StopRequestAction
    | FetchPlayersAction
    | FetchPostsAction
    | FilterByTagAction
    | ResfreshPostsAction;

export {
    RootState,
    IAppState,
    IFilterByTag,
    Player,
    Gun,
    Post,
    Tag,
    Achievement,
    ActionTypes,
    RelatedPlayer,
    FetchPostsAction,
    FilterByTagAction,
    ResfreshPostsAction,
    REFRESH_POSTS,
    FILTER_BY_TAG,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_POSTS_DATA
};
