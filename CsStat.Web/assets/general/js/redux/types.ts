
interface IAppState {
    isLoading: boolean,
    selectedPlayer: ''
    DateFrom: string
    DateTo: string
    players: IPlayer[]
};
interface IPlayer {
    id: string
    Name: string
    ImagePath: string
    Points: number
    Kills: number
    Deaths: number
    Assists: number
    FriendlyKills: number
    KillsPerGame: number
    AssistsPerGame: number
    DeathsPerGame: number
    DefusedBombs: number
    ExplodedBombs: number
    TotalGames: number
    HeadShot: number
    KdRatio: number
    Achievements: IAchievements[]
    Guns: IGuns[]
}

interface IAchievements {
    Id: number
    Name: string
    Description: string
}
interface IGuns { 
    Id: number
    Name: string
    Kills: string
}

const SELECT_PLAYER = 'SELECT_PLAYER';
const FETCH_PLAYERS_DATA = 'FETCH_PLAYERS_DATA';
const START_REQUEST = 'START_REQUEST';


interface SelectPlayerAction {
    type: typeof SELECT_PLAYER
    payload: string
}
  
interface StartRequestAction {
    type: typeof START_REQUEST
}
  
interface FetchPlayersAction {
    type: typeof FETCH_PLAYERS_DATA
    payload: IAppState
}

type ActionTypes = SelectPlayerAction | StartRequestAction | FetchPlayersAction;

export {IAppState, ActionTypes, SELECT_PLAYER, FETCH_PLAYERS_DATA, START_REQUEST}