import React from 'react';
import { Link } from 'react-router-dom';
import { parseISO, format } from 'date-fns';
import { BaseMatchModel } from 'models';

interface MatchesCardProps {
    match: BaseMatchModel | null,
}

export const MatchesCard: React.FC<MatchesCardProps> = ({ match }) => {
    if (!match) return null;

    const hasHalfPeriod = match.aScore + match.bScore > 15;
    const teamACss = hasHalfPeriod ? 'color-ct-primary' : 'color-t-primary';
    const teamBCss = hasHalfPeriod ? 'color-t-primary' : 'color-ct-primary';

    return (
        <Link className="matches-card" to={`/matches/${match.id}`}>
            <div className="matches-card__header">
                <img className="matches-card__image" src={match.mapImage} />
                <div className="matches-card__date">{format(parseISO(match.date), 'dd MMM HH:mm')}</div>
            </div>
            <div className="matches-card__main">
                <h3 className="matches-card__title">{match.map}</h3>
                <div className="matches-card__score">
                    <span className={teamACss}>{hasHalfPeriod ? match.bScore : match.aScore }</span>
                    <span className="matches-card__colon">:</span>
                    <span className={teamBCss}>{hasHalfPeriod ? match.aScore : match.bScore }</span>
                </div>
                {/*<div className="matches-card__duration">{match.duration}</div>*/}
            </div>
        </Link>
    );
};
