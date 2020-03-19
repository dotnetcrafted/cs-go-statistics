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

interface Matches {
    matches: BaseMatch[],
}

interface BaseMatch {
    id: string;
    date: string, // ISO
    map: string, // de_inferno
    mapImage: string, // soruce from CMS
    tScore: number, // 16
    ctScore: number, // 12
    duration?: number, // 129 - total in seconds
}

interface MatchDetails extends BaseMatch {
    rounds: MatchRound[],
}

interface MatchRound {
    id: number,
    tScore: number, // 1
    ctScore: number, // 0
    reason: number, // 2 - bomb planted, etc
    squads: MatchDetailsSquad[],
    kills: MatchDedailsKill[],
}

interface MatchDetailsSquad {
    id: string, // t
    title: string, // Team A
    players: MatchDetailsSquadPlayer[]
}

interface MatchDetailsSquadPlayer {
    id: string, // steamId
    name: string, // djoony
    steamImage: string, // url to steam profile image
    kad: string, // 10/2/5,
    kdDiff: number, // -2
    kd: number, // 1.24
    adr: number, // 118,
    ud: number, // 24,
}

interface MatchDedailsKill {
    id: string,
    formattedTime: string, // 0:28
    killer: string, // MatchDetailsSquadPlayer id
    victim: string, // MatchDetailsSquadPlayer id,
    assister: string,// MatchDetailsSquadPlayer id
    weapon: string, // "USP-S"
    isHeadshot: boolean, 
    isSuicide: boolean,
}

const FETCH_PLAYERS_DATA = 'FETCH_PLAYERS_DATA';
const FETCH_POSTS_DATA = 'FETCH_POSTS_DATA';
const FECTH_MATCHES_DATA = 'FETCH_MATCHES_DATA';
const START_REQUEST = 'START_REQUEST';
const STOP_REQUEST = 'STOP_REQUEST';

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

type FetchMatchesAction = {
    type: typeof FECTH_MATCHES_DATA;
    payload: Matches;
};

type FetchMatchesDetailsAction = {
    type: typeof FECTH_MATCHES_DATA;
    payload: MatchDetails;
};

type ActionTypes =
    | StartRequestAction
    | StopRequestAction
    | FetchPlayersAction
    | FetchPostsAction
    | FetchMatchesAction
    | FetchMatchesDetailsAction;

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
    FetchMatchesAction,
    FetchMatchesDetailsAction,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_PLAYERS_DATA,
    FETCH_POSTS_DATA,
    FECTH_MATCHES_DATA,
};
