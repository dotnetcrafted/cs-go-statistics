import React from 'react';

const UPDATE_IN_SEC = 15;

class ServerInfo extends React.Component {
    intervalId: number;

    constructor(props: any){
        super(props);

        this.state = {
            data: null,
        }

        this.intervalId = -1;
    }

    componentDidMount() {
        this.fetchData();
        this.intervalId = window.setInterval(() => this.fetchData(), UPDATE_IN_SEC * 1000);
    }

    componentWillUnmount() {
        clearInterval(this.intervalId);
    }

    fetchData = (): void => {
        fetch('/api/serverinfo')
            .then((res: Response) => res.json())
            .then((data: any) => {
                this.setState({
                    data,
                })
            })
            .catch(() => {
                this.setState({
                    data: null,
                })
            });
    }

    render() {
        const { data } = this.state;

        if (!data) {
            return null;
        }

        return (
            <div className="server-info">
                <div>
                    {
                        data.ImageUrl &&
                        <div className="server-info__map">
                            <img src={data.ImageUrl} />
                        </div>
                    }
                    {
                        data.IsAlive &&
                            <>
                                <div className="server-info__name">{data.Map}</div>
                                <div className="server-info__players">{data.PlayersCount}</div>
                            </>
                    }
                </div>

            </div>
        )
    }
}

export default ServerInfo;