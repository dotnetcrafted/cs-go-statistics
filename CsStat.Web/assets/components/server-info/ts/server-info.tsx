import React from 'react';

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
        this.intervalId = window.setInterval(() => this.fetchData, 15000);
    }

    componentWillUnmount() {
        clearInterval(this.intervalId);
    }

    fetchData = (): void => {
        fetch('/api/serverinfo')
            .then((res: Response) => res.json())
            .then((data: any) => {
                console.log(data);
                this.setState({
                    data
                })
            })
            .catch((error) => {
                console.log('error', error);
            });
    }

    render() {
        const { data } = this.state;

        if (!data || !data.IsAlive) {
            return <div className="server-info">Server is down</div>;
        }

        return (
            <div className="server-info">
                <img className="server-info__map" src="//picsum.photos/id/1025/128/128" alt="Map Name"/>
                <div className="server-info__players">
                    <span className="server-info__connected">{data.PlayersCount}</span>
                    <span className="server-info__separator">/</span>
                    <span className="server-info__total">20</span>
                </div>
            </div>
        )
    }
}

export default ServerInfo;