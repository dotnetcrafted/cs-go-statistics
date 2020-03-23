import React from 'react';
import { connect } from 'react-redux';
import { createMatchSelector } from 'connected-react-router';
import { MatchDetails } from 'general/ts/redux/types';
import MatchDetailsLayout from './match-details-layout';

interface MatchDetailsControllerState {
    match: MatchDetails | null,
    selectedRoundId: number | null,
}

class MatchDetailsController extends React.Component<any, MatchDetailsControllerState> {
    constructor(props: any) {
        super(props);
        this.state = {
            match: null,
            selectedRoundId: null,
        }
    }

    componentDidMount() {
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
            })
    }

    selectRound = (roundId: number) => {
        this.setState({ selectedRoundId: roundId });
    }

    getRound() {
        const { match } = this.state;

        if (!match || !Array.isArray(match.rounds) || !match.rounds.length) return null;

        let roundId = this.state.selectedRoundId;

        if (!roundId) {
            return match.rounds[match.rounds.length - 1];
        }

        const foundRound = match.rounds.find((round) => round.id === roundId);

        return foundRound || null;
    }


    render() {
        return (
            <MatchDetailsLayout
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

export const MatchDetailsControllerConnected = connect(
    mapStateToProps,
    null
)(MatchDetailsController);