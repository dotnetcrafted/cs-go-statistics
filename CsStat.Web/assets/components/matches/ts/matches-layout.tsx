import React from 'react';
import { MatchesCard } from './macthes-card';

const MatchesLayout = ({ matches }: any) => {
    if (!Array.isArray(matches)) return null;

    return (
        <div className="matches">
            <div className="container">
                <h1>Matches</h1>
                <div>Filters</div>
                <ul className="matches__list">
                    {
                        matches.map((match) => {
                            return (
                                <li className="matches__li" key={match.id}>
                                    <MatchesCard match={match} />
                                </li>
                            );
                        })
                    }
                </ul>
            </div>
        </div>
    );
}

export default MatchesLayout;