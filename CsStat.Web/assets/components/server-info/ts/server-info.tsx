import React, { ReactNode } from 'react';

const UPDATE_IN_SEC = 15;

type ServerInfoState = {
    data: {
        imageUrl: string;
        map: string;
        isAlive: boolean;
        playersCount: number;
    } | null
};

export class ServerInfo extends React.Component<any, ServerInfoState> {
    intervalId: number;

    constructor(props: any) {
        super(props);

        this.state = {
            data: null,
        };

        this.intervalId = -1;
    }

    componentDidMount(): void {
        this.fetchData();
        this.intervalId = window.setInterval(() => this.fetchData(), UPDATE_IN_SEC * 1000);
    }

    componentWillUnmount(): void {
        clearInterval(this.intervalId);
    }

    fetchData = (): void => {
        fetch('/api/bot/serverinfo')
            .then((res: Response) => res.json())
            .then((data: any) => {
                this.setState({
                    data,
                });
            })
            .catch(() => {
                this.setState({
                    data: null,
                });
            });
    };

    render(): ReactNode {
        const { data } = this.state;

        return (
            <div className="server-info">
                {data ? (
                    <div>
                        {data.imageUrl && (
                            <div className="server-info__map">
                                <img src={data.imageUrl} alt="" />
                            </div>
                        )}
                        <div className="server-info__name">{data.map}</div>
                        {data.isAlive && (
                            <div className="server-info__players">
                                {data.playersCount}
                            </div>
                        )}
                    </div>
                ) : null}
            </div>
        );
    }
}
