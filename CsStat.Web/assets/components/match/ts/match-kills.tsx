import React, { ReactNode } from 'react';
import { MatchSquadModel, MatchPlayerModel, MatchKillModel } from 'models';
import { getWeaponById, getIconByName, getPlayerById } from 'project/helpers';

export class MatchKills extends React.Component<any, {}> {
    getPlayerById(id: string): MatchPlayerModel | null {
        const { round } = this.props;

        if (!id) return null;

        let foundPlayer: MatchPlayerModel | null = null;

        round.squads.forEach((squad: MatchSquadModel) => {
            squad.players.forEach((player: MatchPlayerModel) => {
                if (player.id.toString() === id.toString()) {
                    foundPlayer = player;
                }
            });
        });

        return foundPlayer || null;
    }

    renderPlayer(id: string): ReactNode | null {
        const player = this.getPlayerById(id);
        const cmsPlayer = getPlayerById(id);

        if (!player || !cmsPlayer) return null;

        const playerCss = player.team === 'Terrorist' ? 't' : 'ct';

        return (
            <span className={`match-kills__player ${playerCss}`}>
                {cmsPlayer.nickName}
            </span>
        );
    }

    renderAssister(kill: MatchKillModel): ReactNode | null {
        if (!kill.assister) return null;

        return (
            <>
                &nbsp;<b>+</b>&nbsp;
                {this.renderPlayer(kill.assister)}
            </>
        );
    }

    renderWeapon(id: number): ReactNode | string {
        const weapon = getWeaponById(id);

        if (!weapon) return 'weapon';

        return (
            <img
                className="match-kills__weapon"
                src={weapon.iconImage}
                alt={weapon.name}
                title={weapon.name}
            />
        );
    }

    renderPenetrationIcon(isPenetrated: boolean): ReactNode | string {
        if (!isPenetrated) return null;

        const penetratedIcon = getIconByName('penetratedIcon');

        if (!penetratedIcon) return 'penetrated';

        return (
            <img
                className="match-kills__penetrated"
                src={penetratedIcon.image}
                alt={'penetrated'}
                title={'Damage incomes through obstacles'}
            />
        );
    }

    renderHeadshotIcon(isHeadshot: boolean): ReactNode | string {
        if (!isHeadshot) return null;

        const headshotIcon = getIconByName('headShotIcon');

        if (!headshotIcon) return 'headshot';

        return (
            <img
                className="match-kills__headshot"
                src={headshotIcon.image}
                alt={'headshot'}
                title={'Damage incomes right in the head'}
            />
        );
    }

    renderSuicideIcon(isSuicide: boolean): ReactNode | string {
        if (!isSuicide) return null;

        const suicideIcon = getIconByName('suicideIcon');

        if (!suicideIcon) return 'suicide';

        return (
            <img
                className="match-kills__suicide"
                src={suicideIcon.image}
                alt={'suicide'}
                title={'Emo'}
            />
        );
    }

    renderTime(time: number): string {
        const minutes = Math.floor(time / 60);
        const formattedMinutes = minutes < 0 ? '0' : minutes;
        const seconds = time - minutes * 60;
        let formattedSeconds = seconds < 10 ? `0${seconds}` : seconds;

        if (seconds === 0) {
            formattedSeconds = '00';
        }

        return `${formattedMinutes}:${formattedSeconds}`;
    }

    render(): ReactNode {
        const { round } = this.props;

        if (!round || !Array.isArray(round.kills)) return null;

        return (
            <div className="match-kills">
                <ul className="match-kills__list">
                    {round.kills.map((kill: MatchKillModel, i: number) => {
                        return (
                            <li
                                className="match-kills__li"
                                key={`${round.id}-${i}`}
                            >
                                <span className="match-kills__log">
                                    {this.renderPlayer(kill.killer)}
                                    {this.renderAssister(kill)}
                                    &nbsp;
                                    {!kill.isSuicide &&
                                        this.renderWeapon(kill.weapon)}
                                    {this.renderPenetrationIcon(
                                        kill.isPenetrated
                                    )}
                                    {this.renderHeadshotIcon(kill.isHeadshot)}
                                    {this.renderSuicideIcon(kill.isSuicide)}
                                    &nbsp;
                                    {this.renderPlayer(kill.victim)}
                                </span>
                                <span className="match-kills__time">
                                    {this.renderTime(kill.time)}
                                </span>
                            </li>
                        );
                    })}
                </ul>
            </div>
        );
    }
}
