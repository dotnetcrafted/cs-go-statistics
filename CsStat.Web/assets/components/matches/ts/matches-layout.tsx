import React from 'react';
import { MatchesModel } from 'models';
import { MatchesCard } from './macthes-card';

interface MatchesLayoutProps {
    matches: MatchesModel | null
}

export const MatchesLayout: React.FC<MatchesLayoutProps> = ({ matches }) => {
    if (!Array.isArray(matches)) return null;

    return (
        <div className="matches">
            <div className="container">
                <h1 className="matches__title">Matches</h1>
                <div className="matches__grid">
                    <ul className="matches__list">
                        {
                            matches.length > 0 ?
                                matches.map((match) => {
                                    return (
                                        <li className="matches__li" key={match.id}>
                                            <MatchesCard match={match} />
                                        </li>
                                    );
                                }) :
                                <p>No matches found</p>
                        }
                    </ul>
                </div>
            </div>
        </div>
    );
};
