import React from 'react';
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
   
    renderEmptyRound(roundIndex: number) {
        return (
            <li className="match-details-rounds__li" key={roundIndex}>
                <a className={`match-details-rounds__col`} href="#"                >
                    <div className={`match-details-rounds__cell match-details-rounds__cell--top`}></div>
                    <div className="match-details-rounds__cell match-details-rounds__cell--mid">{roundIndex}</div>
                    <div className={`match-details-rounds__cell match-details-rounds__cell--bottom`}>                    </div>
                </a>
            </li>
        );
    }

    renderRound(round: MatchRound) {
        const { selectedRoundId, selectRound } = this.props;
        const colCss = selectedRoundId === round.id ? 'is-selected' : '';
        const isAttackReason = this.checkAttackReason(round.reason);
        const topCss = isAttackReason ? 'has-value' : '';
        const bottomCss = !isAttackReason ? 'has-value' : '';

        return (
            <li className="match-details-rounds__li" key={round.id}>
                <a className={`match-details-rounds__col ${colCss}`}
                    href="#"
                    onClick={(event) => {
                        event.preventDefault();
                        selectRound(round.id)
                    }}
                >
                    <div className={`match-details-rounds__cell match-details-rounds__cell--top ${topCss}`}>
                        {isAttackReason && round.reason}
                    </div>
                    <div className="match-details-rounds__cell match-details-rounds__cell--mid">{round.id}</div>
                    <div className={`match-details-rounds__cell match-details-rounds__cell--bottom ${bottomCss}`}>
                        {!isAttackReason && round.reason}
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
            <div className="match-details-rounds">
                <ul className="match-details-rounds__list">
                    {this.renderRounds()}
                </ul>
            </div>

        );
    }
}