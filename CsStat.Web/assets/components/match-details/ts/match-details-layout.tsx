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
            <div className="match__bg of-cover">
                <img src={match.mapImage} />
            </div>
            <div className="container">
                <div className="match__content">
                    <div className="match__header">
                        <h1 className="match__title">{match.map}
                            <span>({match.duration})</span>
                        </h1>
                        <div className="match__score">
                            <span className="color-t-primary">{match.tScore}</span>
                            <span>:</span>
                            <span className="color-ct-primary">{match.ctScore}</span>
                        </div>
                    </div>
                    <MatchDetailsRounds
                        rounds={match.rounds}
                        selectedRoundId={selectedRoundId}
                        selectRound={selectRound}
                    />
                    <div className="match__stats">
                        <MatchDetailsStats round={round} />
                    </div>
                    <div className="match__kills">
                        <MatchDetailsKills round={round} />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default MatchDetailsLayout;