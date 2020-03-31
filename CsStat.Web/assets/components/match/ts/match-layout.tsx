import React from 'react';
import { MatchModel, MatchRoundModel } from 'models';
import { MatchRounds } from './match-rounds';
import { MatchStats } from './match-stats';
import { MatchKills } from './match-kills';

interface MatchDetailsLayoutProps {
    match: MatchModel | null,
    round: MatchRoundModel | null,
    selectedRoundId: number | null,
    selectRound: any,
}

const MatchLayout: React.FC<MatchDetailsLayoutProps> = ({ match, round, selectedRoundId, selectRound }) => {
    if (!match) return null;

    const hasHalfPeriod = match.aScore + match.bScore > 15;
    const teamACss = hasHalfPeriod ? 'color-ct-primary' : 'color-t-primary';
    const teamBCss = hasHalfPeriod ? 'color-t-primary' : 'color-ct-primary';

    return (
        <div className="match">
            <div className="match__bg of-cover">
                <img src={match.mapImage} alt={match.map} />
            </div>
            <div className="container">
                <div className="match__content">
                    <div className="match__header">
                        <div className="match__title">
                            <h1 className="match__map">{match.map}</h1>
                            {/* <span>({match.duration})</span> */}
                        </div>
                        <div className="match__score">
                            <span className={teamACss}>{hasHalfPeriod ? match.bScore : match.aScore }</span>
                            <span className="match__score-colon">:</span>
                            <span className={teamBCss}>{hasHalfPeriod ? match.aScore: match.bScore}</span>
                        </div>
                    </div>
                    <MatchRounds
                        rounds={match.rounds}
                        selectedRoundId={selectedRoundId}
                        selectRound={selectRound}
                    />
                    <div className="match__body">
                        <div className="match__stats">
                            <MatchStats round={round} />
                        </div>
                        <div className="match__kills">
                            <MatchKills round={round} />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MatchLayout;
