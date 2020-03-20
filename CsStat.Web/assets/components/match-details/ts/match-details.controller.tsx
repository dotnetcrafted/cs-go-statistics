import React from 'react';
import MatchDetailsLayout from './match-details-layout';

export class MatchDetailsController extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
        this.state = {
            match: null,
        }
    }

    componentDidMount() {
        fetch('/api/matchdata?matchId=5e71c12a58daf2008805cb58')
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

    render() {
        return (
            <MatchDetailsLayout match={this.state.match} />
        );
    }
}
