import React from 'react';
import { MatchRound } from 'general/ts/redux/types';

interface MatchDetailsRoundsProps {
    rounds: MatchRound[],
}

export const MatchDetailsRounds = ({ rounds }: MatchDetailsRoundsProps) => {
    if (!Array.isArray(rounds)) return null;

    return (
        <div className="match-details-rounds">
            <ul className="match-details-rounds__list">
                {
                    rounds.map((round: MatchRound) => {
                        return (
                            <li className="match-details-rounds__li" key={round.id}>
                                <div className="match-details-rounds__cell match-details-rounds__cell--t">{round.tScore}</div>
                                <div className="match-details-rounds__cell match-details-rounds__cell--mid">{round.id}</div>
                                <div className="match-details-rounds__cell match-details-rounds__cell--ct">{round.ctScore}</div>
                                {/* <div>{round.reason}</div> */}
                            </li>
                        );
                    })
                }
            </ul>
        </div>

    );
}