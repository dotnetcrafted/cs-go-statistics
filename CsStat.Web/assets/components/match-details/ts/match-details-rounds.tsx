import React from 'react';
import { MatchRound } from 'general/ts/redux/types';

interface MatchDetailsRoundsProps {
    rounds: MatchRound[],
    selectedRoundId: number | null,
    selectRound: any,
}

export const MatchDetailsRounds = (
    {
        rounds, 
        selectedRoundId,
        selectRound,
    }: MatchDetailsRoundsProps
) => {
    if (!Array.isArray(rounds)) return null;

    return (
        <div className="match-details-rounds">
            <ul className="match-details-rounds__list">
                {
                    rounds.map((round: MatchRound) => {
                        const colCss = selectedRoundId === round.id ? 'is-selected' : '';

                        return (
                            <li className="match-details-rounds__li" key={round.id}>
                                <a className={`match-details-rounds__col ${colCss}`}
                                    href="#"
                                    onClick={(event) => {
                                        event.preventDefault();
                                        selectRound(round.id)
                                    }}
                                >
                                    <div className="match-details-rounds__cell match-details-rounds__cell--t">{round.tScore}</div>
                                    <div className="match-details-rounds__cell match-details-rounds__cell--mid">{round.id}</div>
                                    <div className="match-details-rounds__cell match-details-rounds__cell--ct">{round.ctScore}</div>
                                </a>
                                {/* <div>{round.reason}</div> */}
                            </li>
                        );
                    })
                }
            </ul>
        </div>

    );
}