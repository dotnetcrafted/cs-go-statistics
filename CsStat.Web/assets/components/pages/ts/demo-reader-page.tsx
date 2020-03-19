import '../scss/index.scss';
import axios from 'axios';
import React, { ReactNode } from 'react';
import { connect } from 'react-redux';
import { fetchPosts, startRequest, stopRequest } from '../../../general/ts/redux/actions';
import { RootState } from '../../../general/ts/redux/types';

interface MatchDetailsState {
    match: any,
}

class DemoReaderPage extends React.Component<DemoReaderPageProps, MatchDetailsState> {
    constructor(props: DemoReaderPageProps) {
        super(props);
        this.state = {
            match: null,
        }
    }
    componentDidMount() {
        axios
            .get('/api/matchdata?matchId=5e42765b58daf2b44cbb4527')
            .then((response) => {
                this.setState({
                    match: response.data.match
                })
                console.log(response.data.match);
            });
    }

    renderRounds() {
        const { match } = this.state;

        if (!match || !match.Rounds) return null;

        return match.Rounds.map((round: any) => {
            return (
                <div>
                    {
                        <div>{round.Winner === 1 && round.ReasonTitle || '0'}</div>
                    }
                    <div>{round.RoundNumber}</div>
                    {
                        <div>{round.Winner === 2 && round.ReasonTitle || '0'}</div>
                    }
                </div>
            )
            ;
        })
    }

    render(): ReactNode {
        return (
            <div className="demo-reader-page">
                <h1>Demo Reader Page</h1>
                <a href={this.props.MatchesDataApiPath}>API URL</a>
                <div style={{display: 'flex'}}>
                    {this.renderRounds()}
                </div>
            </div>
        );
    }
}

type DemoReaderPageProps = {
    IsLoading: boolean;
    MatchesDataApiPath: string;
};

const mapStateToProps = (state: RootState) => {
    const IsLoading = state.app.IsLoading;
    return { IsLoading };
};

export default connect(
    mapStateToProps,
    { fetchPosts, startRequest, stopRequest }
)(DemoReaderPage);
