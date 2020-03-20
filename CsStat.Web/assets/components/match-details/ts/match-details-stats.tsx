import React from 'react';

export const MatchDetailsStats = ({ squads }: any) => {
    if (!Array.isArray(squads)) return null;

    return (
        <div>
            { 
                squads.map((squad) => {
                    return squad.id;
                }) 
            }
        </div>
    );
}