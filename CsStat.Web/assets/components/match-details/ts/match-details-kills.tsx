import React from 'react';
import {
    MatchDetailsSquad,
    MatchDetailsSquadPlayer,
    MatchDetailsKill
} from 'general/ts/redux/types';

export class MatchDetailsKills extends React.Component<any, {}> {
    getPlayerById(id: string) {
        const { round } = this.props;

        if (!id) return null;

        let foundPlayer: MatchDetailsSquadPlayer | undefined;

        round.squads.forEach((squad: MatchDetailsSquad) => {
            const team = squad.title;
            squad.players.forEach((player: MatchDetailsSquadPlayer) => {
                if (player.id.toString() === id.toString()) {
                    foundPlayer = {
                        ...player,
                        team
                    };
                }
            });
        });

        return foundPlayer || null;
    }

    renderPlayer(id: string) {
        const player = this.getPlayerById(id);

        if (!player) return null;

        const playerCss = player.team === 'Team A' ? 'color-t-primary' : 'color-ct-primary';

        return <span className={`match-kills__player ${playerCss}`}>{player.name}</span>
    }

    renderWeapon() {
        return <img
            className="match-kills__weapon"
            src='https://admin.csfuse8.site/uploads/4f940096703b4100b751a9127c1acea4.png'
            alt=""
        />;
    }

    render() {
        const { round } = this.props;

        if (!round || !Array.isArray(round.kills)) return null;

        return (
            <div className="match-kills">
                <ul className="match-kills__list">
                    {
                        round.kills.map((kill: MatchDetailsKill, i: number) => {
                            const weaponIcon = kill.weapon;
                            const headshotIcon = kill.isHeadshot && 'HS';

                            return (
                                <li className="match-kills__li" key={`${round.id}-${i}`}>
                                    {this.renderPlayer(kill.killer)}
                                    &nbsp;
                                    {this.renderPlayer(kill.assister)}
                                    &nbsp;
                                    {this.renderWeapon()}
                                    &nbsp;
                                    {headshotIcon}
                                    &nbsp;
                                    {this.renderPlayer(kill.victim)}
                                </li>
                            );
                        })
                    }
                </ul>
            </div>
        );
    }
}