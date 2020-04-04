import React, { ReactNode } from 'react';
import { MatchesModel } from 'models';
import { MatchesLayout } from './matches-layout';

interface MatchesControllerState {
    matches: MatchesModel | null,
}

export class MatchesController extends React.Component<any, MatchesControllerState> {
    constructor(props: any) {
        super(props);
        this.state = {
            matches: null,
        };
    }

    componentDidMount(): void {
        fetch('/api/matchesdata')
            .then((res: Response) => res.json())
            .then((data) => {
                this.setState({
                    matches: data
                });
            })
            .catch((err) => {
                console.log(err);
            });
    }

    render(): ReactNode {
        return (
            <MatchesLayout matches={this.state.matches} />
        );
    }
}
