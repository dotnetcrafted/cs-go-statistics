import React from 'react';
import { MatchDetails } from 'general/ts/redux/types';
import { MatchDetailsRounds } from './match-details-rounds';
import { MatchDetailsStats } from './match-details-stats';
import { MatchDetailsKills } from './match-details-kills';

interface MatchDetailsLayoutProps {
    match: MatchDetails,
}

const MatchDetailsLayout = ({ match } : MatchDetailsLayoutProps) => {
    if (!match) return null;

    console.log(match);

    const round = match.rounds[match.rounds.length - 3];
    console.log(round);

    return (
        <div>
            <div className="container">
                <h1>Match Details</h1>
                <div>{match.map}</div>
                <MatchDetailsRounds rounds={match.rounds} />
                <MatchDetailsStats squads={round.squads} />
                <MatchDetailsKills round={round} />
            </div>

        </div>
    );
}

export default MatchDetailsLayout;