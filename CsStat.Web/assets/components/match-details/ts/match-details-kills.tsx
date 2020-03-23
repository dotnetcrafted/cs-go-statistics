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

    renderAssister(kill: MatchDetailsKill) {
        if (!kill.assister) return null;

        return (
            <>
                &nbsp;<b>+</b>&nbsp;
                {this.renderPlayer(kill.assister)}
            </>
        );
    }

    renderWeapon() {
        return <img
            className="match-kills__weapon"
            src='https://admin.csfuse8.site/uploads/4f940096703b4100b751a9127c1acea4.png'
            alt=""
        />;
    }

    renderPenetrationIcon(kill: MatchDetailsKill) {
        if (!kill.isPenetrated) return null

        return (
            <>
                &nbsp;
                PROSTREL
            </>
        );
    }

    renderHeadshotIcon(kill: MatchDetailsKill) {
        if (!kill.isHeadshot) return null;

        return (
            <>
                &nbsp;
                HEADSHOT
            </>
        );
    }

    renderTime(time: number) {
        if (Math.ceil(time/60) < 0) {
            return `0:${time}`;
        }

        const minutes = Math.floor(time/60);

        return `${minutes}:${time - (minutes * 60)}`;
    }

    render() {
        const { round } = this.props;

        if (!round || !Array.isArray(round.kills)) return null;

        return (
            <div className="match-kills">
                <ul className="match-kills__list">
                    {
                        round.kills.map((kill: MatchDetailsKill, i: number) => {
                            return (
                                <li className="match-kills__li" key={`${round.id}-${i}`}>
                                    <span className="match-kills__log">
                                        {this.renderPlayer(kill.killer)}
                                        {this.renderAssister(kill)}
                                        &nbsp;
                                        {this.renderWeapon()}
                                        {this.renderPenetrationIcon(kill)}
                                        {this.renderHeadshotIcon(kill)}
                                        &nbsp;
                                        {this.renderPlayer(kill.victim)}
                                    </span>
                                    <span className="match-kills__time">
                                        {this.renderTime(kill.time)}
                                    </span>
                                </li>
                            );
                        })
                    }
                </ul>
            </div>
        );
    }
}