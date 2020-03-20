import React from 'react';
import {
    MatchDetailsSquad,
    MatchDetailsSquadPlayer,
    MatchDetailsKill
} from 'general/ts/redux/types';

export class MatchDetailsKills extends React.Component<any, {}> {
    getPlayerNameById(id: string) {
        const { round } = this.props;

        if (!id) return null;

        let foundPlayer: MatchDetailsSquadPlayer | undefined;

        round.squads.forEach((squad: MatchDetailsSquad) => {
            squad.players.forEach((player: MatchDetailsSquadPlayer) => {
                if (player.id === id) {
                    foundPlayer = player;
                }
            });
        });

        return foundPlayer ? foundPlayer.name : null;
    }

    render() {
        const { round } = this.props;

        if (!round || !Array.isArray(round.kills)) return null;

        return (
            <div>
                <ul>
                    {
                        round.kills.map((kill: MatchDetailsKill) => {
                            const killer = this.getPlayerNameById(kill.killer);
                            const assister = this.getPlayerNameById(kill.assister);
                            const weaponIcon = kill.weapon;
                            const headshotIcon = kill.isHeadshot && 'HS';
                            const victim = this.getPlayerNameById(kill.victim);

                            console.log(kill.isHeadshot);


                            return (
                                <li key={kill.id}>
                                    {killer}
                                    &nbsp;
                                    {assister}
                                    &nbsp;
                                    {weaponIcon}
                                    &nbsp;
                                    {headshotIcon}
                                    &nbsp;
                                    {victim}
                                </li>
                            );
                        })
                    }
                </ul>
            </div>
        );
    }
}