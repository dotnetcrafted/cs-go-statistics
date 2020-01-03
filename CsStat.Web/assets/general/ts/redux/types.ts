import { RouterState } from 'connected-react-router';

type AppState = {
    router?: RouterState;
    data: {
        IsLoading: boolean;
        SelectedPlayer: string;
        DateFrom: string;
        DateTo: string;
        Players: Player[];
    }
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
    Id: number;
    Name: string;
    Description: string;
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

const SELECT_PLAYER = 'SELECT_PLAYER';
const FETCH_PLAYERS_DATA = 'FETCH_PLAYERS_DATA';
const START_REQUEST = 'START_REQUEST';
const STOP_REQUEST = 'STOP_REQUEST';

type SelectPlayerAction = {
    type: typeof SELECT_PLAYER;
    payload: string;
};

type StartRequestAction = {
    type: typeof START_REQUEST;
};

type StopRequestAction = {
    type: typeof STOP_REQUEST;
};

type FetchPlayersAction = {
    type: typeof FETCH_PLAYERS_DATA;
    payload: AppState;
};

type ActionTypes =
    | SelectPlayerAction
    | StartRequestAction
    | StopRequestAction
    | FetchPlayersAction;

export {
    AppState,
    Player,
    Gun,
    Achievement,
    ActionTypes,
    RelatedPlayer,
    SELECT_PLAYER,
    FETCH_PLAYERS_DATA,
    START_REQUEST,
    STOP_REQUEST
};
