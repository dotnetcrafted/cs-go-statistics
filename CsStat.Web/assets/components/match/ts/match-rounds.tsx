import React, { ReactNode } from 'react';
import { MatchRoundModel } from 'models';
import { getIconByName } from 'project/helpers';

interface MatchDetailsRoundsProps {
    rounds: MatchRoundModel[],
    selectedRoundId: number | null,
    selectRound: any,
}

export class MatchRounds extends React.Component<MatchDetailsRoundsProps, {}> {
    checkAttackReason(reason: number): boolean {
        return reason === 1 || reason === 9;
    }

    renderReasonIcon(reason: string): string | ReactNode {
        const reasonIcon = getIconByName(reason);

        if (!reasonIcon) return reason;

        return <img className="match-rounds__icon" src={reasonIcon.image} alt={reason} title={reason} />;
    }

    renderEmptyRound(roundIndex: number): ReactNode {
        return (
            <li className="match-rounds__li" key={roundIndex}>
                <a className={'match-rounds__col'}>
                    <div className="match-rounds__cell match-rounds__cell--top no-value" />
                    <div className="match-rounds__cell match-rounds__cell--mid">{roundIndex}</div>
                    <div className="match-rounds__cell match-rounds__cell--bottom no-value" />
                </a>
            </li>
        );
    }

    renderRound(round: MatchRoundModel): ReactNode {
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
                        selectRound(round.id);
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

    getRound(roundIndex: number): ReactNode {
        const { rounds } = this.props;
        if (rounds[roundIndex - 1]) return this.renderRound(rounds[roundIndex - 1]);

        return this.renderEmptyRound(roundIndex);
    }

    renderRounds(): ReactNode[] {
        const roundsToRender = [];

        for (let i = 1; i <= 30; i++) {
            roundsToRender.push(this.getRound(i));
        }

        return roundsToRender;
    }

    render(): ReactNode {
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
