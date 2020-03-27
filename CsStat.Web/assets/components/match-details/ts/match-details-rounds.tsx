import React from 'react';
import { getIconByName } from 'project/helpers';
import { MatchRound } from 'general/ts/redux/types';

interface MatchDetailsRoundsProps {
    rounds: MatchRound[],
    selectedRoundId: number | null,
    selectRound: any,
}

export class MatchDetailsRounds extends React.Component<MatchDetailsRoundsProps, {}> {
    checkAttackReason(reason: number) {
        if (reason === 1 || reason === 9) return true;

        return false;
    }

    renderReasonIcon(reason: string) {
        const reasonIcon = getIconByName(reason);

        if (!reasonIcon) return reason;

        return <img className="match-rounds__icon" src={reasonIcon.image} alt={reason} title={reason} />
    }
   
    renderEmptyRound(roundIndex: number) {
        return (
            <li className="match-rounds__li" key={roundIndex}>
                <a className={`match-rounds__col`}>
                    <div className="match-rounds__cell match-rounds__cell--top no-value"></div>
                    <div className="match-rounds__cell match-rounds__cell--mid">{roundIndex}</div>
                    <div className="match-rounds__cell match-rounds__cell--bottom no-value"></div>
                </a>
            </li>
        );
    }

    renderRound(round: MatchRound) {
        const { selectedRoundId, selectRound } = this.props;
        const colCss = selectedRoundId === round.id ? 'is-selected' : '';
        const isAttackReason = this.checkAttackReason(round.reason);
        const topCss = isAttackReason ? 'color-t-primary' : 'no-value';
        const bottomCss = !isAttackReason ? 'color-ct-primary' : 'no-value';
        const attackReasonIcon = isAttackReason && this.renderReasonIcon(round.reasonTitle);
        const defenceReasonIcon = !isAttackReason && this.renderReasonIcon(round.reasonTitle);

        return (
            <li className="match-rounds__li" key={round.id}>
                <a className={`match-rounds__col ${colCss}`}
                    onClick={(event) => {
                        event.preventDefault();
                        selectRound(round.id)
                    }}
                >
                    <div className={`match-rounds__cell match-rounds__cell--top ${round.id <= 15 ? topCss : bottomCss}`}>
                        {round.id <= 15 ? attackReasonIcon : defenceReasonIcon}
                    </div>
                    <div className="match-rounds__cell match-rounds__cell--mid">{round.id}</div>
                    <div className={`match-rounds__cell match-rounds__cell--bottom ${round.id <= 15 ? bottomCss : topCss}`}>
                        {round.id <= 15 ? defenceReasonIcon : attackReasonIcon}
                    </div>
                </a>
            </li>
        );
    }

    getRound(roundIndex: number) {
        const { rounds } = this.props;
        if (rounds[roundIndex - 1]) return this.renderRound(rounds[roundIndex -1]);

        return this.renderEmptyRound(roundIndex);
    }

    renderRounds() {
        const roundsToRender = [];

        for (let i = 1; i <= 30; i++) {
            roundsToRender.push(this.getRound(i));
        }

        return roundsToRender;
    }

    render() {
        const { rounds } = this.props;

        if (!Array.isArray(rounds)) return null;

        return (
            <div className="match-rounds">
                <ul className="match-rounds__list">
                    {this.renderRounds()}
                </ul>
            </div>

        );
    }
}