import React from 'react';
import { MatchDetails, MatchRound } from 'general/ts/redux/types';
import { MatchDetailsRounds } from './match-details-rounds';
import { MatchDetailsStats } from './match-details-stats';
import { MatchDetailsKills } from './match-details-kills';

interface MatchDetailsLayoutProps {
    match: MatchDetails | null,
    round: MatchRound | null,
    selectedRoundId: number | null,
    selectRound: any,
}

const MatchDetailsLayout = ({ match, round, selectedRoundId, selectRound }: MatchDetailsLayoutProps) => {
    if (!match) return null;

    //console.log(match);
    //console.log(round);

    return (
        <div className="match">
            <div className="container">
                <h1>Match Details</h1>
                <div>{match.map}</div>
                <MatchDetailsRounds
                    rounds={match.rounds}
                    selectedRoundId={selectedRoundId}
                    selectRound={selectRound}
                />
                <MatchDetailsStats squads={round} />
                <div className="match__kills">
                    <MatchDetailsKills round={round} />
                </div>
            </div>
        </div>
    );
}

export default MatchDetailsLayout;