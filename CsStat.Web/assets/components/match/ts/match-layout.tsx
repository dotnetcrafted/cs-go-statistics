import React from 'react';
import { parseISO, format } from 'date-fns';
import { MatchModel, MatchRoundModel } from 'models';
import { getDuration } from 'project/helpers';
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

    const duration = getDuration(match.duration);

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
                        </div>
                        <div className="match__score">
                            <div className="match__score-t color-t-primary">{match.aScore}</div>
                            <div className="match__score-colon">:</div>
                            <div className="match__score-ct color-ct-primary">{match.bScore}</div>
                        </div>
                        <div className="match__info">
                            <div className="match__info-date">{format(parseISO(match.date), 'dd MMM HH:mm')}</div>
                            <div className="match__info-duration">
                                {
                                    duration.hours ?
                                        `${duration.hours} h ${duration.minutes} min` :
                                        `${duration.minutes} min`
                                }
                            </div>
                        </div>
                    </div>
                    <div className="match__rounds">
                        <MatchRounds
                            rounds={match.rounds}
                            selectedRoundId={selectedRoundId}
                            selectRound={selectRound}
                        />
                    </div>
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
