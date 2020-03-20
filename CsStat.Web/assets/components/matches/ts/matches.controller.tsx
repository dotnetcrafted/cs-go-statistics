import React from 'react';
import MatchesLayout from './matches-layout';

//const MatchesControllerState

class MatchesController extends React.Component<any, any> {
    constructor(props: any) {
        super(props);
        this.state = {
            macthes: null,
        }
    }
    componentDidMount() {
        fetch('/api/matchesdata')
            .then((res: Response) => res.json())
            .then((data) => {
                this.setState({
                    matches: data
                });
            })
            .catch((err) => {
                console.log(err);
            })
    }

    render() {
        return (
            <MatchesLayout matches={this.state.matches} />
        );
    }
}

export default MatchesController;