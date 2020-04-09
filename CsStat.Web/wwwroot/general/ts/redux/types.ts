import { RouterState } from 'connected-react-router';

interface IAppState {
    isLoading: boolean;
    players: Player[];
    posts: Post[];
}

type RootState = {
    router?: RouterState;
    app: IAppState;
}

type Player = {
    id: string;
    steamId: string,
    steamImage: string,
    name: string,
    points: number;
    kills: number;
    deaths: number;
    assists: number;
    friendlyKills: number;
    killsPerGame: number;
    assistsPerGame: number;
    deathsPerGame: number;
    defusedBombs: number;
    explodedBombs: number;
    totalGames: number;
    headShot: number;
    kdRatio: number;
    kdDif: number;
    kad: string;
    achievements: Achievement[];
    victims: RelatedPlayer[];
    killers: RelatedPlayer[];
    guns: Gun[];
};

type Achievement = {
    achievementId: number;
    name: string;
    description: string;
    iconUrl: string;
};

type Gun = {
    id: number;
    name: string;
    kills: number;
};

type RelatedPlayer = {
    steamId: string,
    count: number;
};

type Post = {
    id: string,
    title: string;
    content: string;
    tags: Tag[];
    createdAt: string;
    updatedAt: string;
}

type Tag = {
    id: string,
    caption: string;
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

// type FetchMatchesAction = {
//     type: typeof FECTH_MATCHES_DATA;
//     payload: Matches;
// };

// type FetchMatchesDetailsAction = {
//     type: typeof FECTH_MATCHES_DATA;
//     payload: MatchDetails;
// };

type ActionTypes =
    | StartRequestAction
    | StopRequestAction
    | FetchPlayersAction
    | FetchPostsAction;
 //   | FetchMatchesAction
 //   | FetchMatchesDetailsAction;

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
    //FetchMatchesAction,
    //FetchMatchesDetailsAction,
    START_REQUEST,
    STOP_REQUEST,
    FETCH_PLAYERS_DATA,
    FETCH_POSTS_DATA,
    FECTH_MATCHES_DATA,
};
