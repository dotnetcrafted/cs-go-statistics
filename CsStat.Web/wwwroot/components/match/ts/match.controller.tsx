import React, { ReactNode } from 'react';
import { connect } from 'react-redux';
import { createMatchSelector } from 'connected-react-router';
import { MatchModel, MatchRoundModel } from 'models';
import MatchLayout from './match-layout';

interface MatchControllerState {
    match: MatchModel | null,
    selectedRoundId: number | null,
}

class MatchController extends React.Component<any, MatchControllerState> {
    constructor(props: any) {
        super(props);
        this.state = {
            match: null,
            selectedRoundId: null,
        };
    }

    componentDidMount(): void {
        const { matchId } = this.props;

        fetch(`/api/matchdata?matchId=${matchId}`)
            .then((res: Response) => res.json())
            .then((data) => {
                this.setState({
                    match: data
                });
            })
            .catch((err) => {
                console.log(err);
            });
    }

    selectRound = (roundId: number): void => {
        this.setState({ selectedRoundId: roundId });
    };

    getRound(): MatchRoundModel | null {
        const { match } = this.state;

        if (!match || !Array.isArray(match.rounds) || !match.rounds.length) return null;

        const roundId = this.state.selectedRoundId;

        if (!roundId) {
            return match.rounds[match.rounds.length - 1];
        }

        return match.rounds.find((round) => round.id === roundId) || null;
    }

    render(): ReactNode {
        return (
            <MatchLayout
                match={this.state.match}
                round={this.getRound()}
                selectedRoundId={this.state.selectedRoundId}
                selectRound={this.selectRound}
            />
        );
    }
}

const mapStateToProps = (state: any) => {
    const matchSelector = createMatchSelector('/matches/:id');
    const match: any = matchSelector(state);
    const matchId = match && match.params.id;

    return {
        matchId,
        router: state.router,
    };
};

export const MatchControllerConnected = connect(
    mapStateToProps,
    null
)(MatchController);
