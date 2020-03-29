export interface MatchesModel {
    matches: BaseMatchModel[],
}

export interface BaseMatchModel {
    id: string;
    date: string,
    map: string,
    mapImage: string,
    tScore: number,
    ctScore: number,
    duration: number,
}

export interface MatchModel extends BaseMatchModel {
    rounds: MatchRoundModel[],
}

export interface MatchRoundModel {
    id: number,
    tScore: number,
    ctScore: number,
    reason: number,
    reasonTitle: string,
    duration: number,
    squads: MatchSquadModel[],
    kills: MatchKillModel[],
}

export interface MatchSquadModel {
    id: string,
    title: string,
    players: MatchPlayerModel[]
}

export interface MatchPlayerModel {
    id: string,
    team: string,
    kills: number,
    assists: number,
    deaths: number,
    kad: string,
    kdDiff: number,
    kd: number,
    adr: number,
    ud: number,
    score: number,
    steamId: string,
    steamImage: string,
    isDied: boolean,
}

export interface MatchKillModel {
    id: string,
    killer: string,
    victim: string,
    assister?: string,
    weapon: number,
    time: number,
    formattedTime: string,
    isHeadshot: boolean,
    isSuicide: boolean,
    isPenetrated: boolean,
}
