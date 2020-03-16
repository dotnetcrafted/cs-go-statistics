import { RouterState } from 'connected-react-router';

interface IAppState {
    IsLoading: boolean;
    Players: Player[];
    Posts: Post[];
}

type RootState = {
    router?: RouterState;
    app: IAppState;
}

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
    KdDif: number;
    Kad: string;
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
}

type Tag = {
    Caption: string;
}

const FETCH_PLAYERS_DATA = 'FETCH_PLAYERS_DATA';
const START_REQUEST = 'START_REQUEST';
const STOP_REQUEST = 'STOP_REQUEST';
const FETCH_POSTS_DATA = 'FETCH_POSTS_DATA';

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
    | FetchPostsAction;

export {
    RootState,
    IAppState,
    Player,
    Gun,
    Post,
    Tag,
    Achievement,
    ActionTypes,
    RelatedPlayer,
    FetchPostsAction,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_POSTS_DATA
};
