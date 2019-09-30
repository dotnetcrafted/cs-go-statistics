import React, {ReactNode} from 'react';
import { Table, Avatar, Divider, Tooltip } from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers, startRequest, selectPlayer } from '../../../general/ts/redux/actions';
import FilterForm, {DateValues} from './filter-form';
import { AppState, Player } from '../../../general/ts/redux/types';

const CELL_CSS_CLASS = 'players-data__cell';

class PlayersData extends React.Component<PlayersDataProps> {
    readonly state = {
        PlayersData: [],
        columns: [
            {
                dataIndex: 'avatar',
                render: (_link: any, record: TablePlayer) => {
                    const content = this.getAvatar(record);
                    return this.cellWrapper(record.key, content); 
                },
                width: '5%',
                className: CELL_CSS_CLASS
            },
            {
                title: 'Player Name',
                dataIndex: 'Name',
                className: CELL_CSS_CLASS,
                render: (_link: any, record: TablePlayer) => {
                    return this.cellWrapper(record.key, record.Name); 
                },
                sorter: (a:TablePlayer, b: TablePlayer) => a.Name.localeCompare(b.Name),
            },
            {
                title: ()=>(
                    <Tooltip title="Kills / Deaths">
                        K/D Ratio
                    </Tooltip>
                ),
                dataIndex: 'KdRatio',
                className: CELL_CSS_CLASS,
                render: (_link: any, record: TablePlayer) => {
                    return this.cellWrapper(record.key, record.KdRatio); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.KdRatio - b.KdRatio,
            },
            {
                title: 'Kills',
                dataIndex: 'Kills',
                className: CELL_CSS_CLASS,
                render: (_link: any, record: TablePlayer) => {
                    return this.cellWrapper(record.key, record.Kills); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.Kills - b.Kills,
            },
            {
                title: 'Deaths',
                dataIndex: 'Deaths',
                className: CELL_CSS_CLASS,
                render: (_link: any, record: TablePlayer) => {
                    return this.cellWrapper(record.key, record.Deaths); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.Deaths - b.Deaths,
            },
            {
                title: 'Total Games',
                dataIndex: 'TotalGames',
                className: CELL_CSS_CLASS,
                render: (_link: any, record: TablePlayer) => {
                    return this.cellWrapper(record.key, record.TotalGames); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.TotalGames - b.TotalGames,
            },
        ],
    };

    componentDidMount() {
        this.fetchPlayers(this.props.playersDataUrl);
    }

    private fetchPlayers(playersDataUrl: string, params?: DateValues) {
        const url = new URL(playersDataUrl, window.location.origin);
        if (params) {
            url.search = new URLSearchParams(params).toString();
        }

        this.props.startRequest();

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: AppState) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;
                this.props.fetchPlayers(data);
            });
        
    }

    private getAvatar(record: TablePlayer) {
        if (record.ImagePath) {
            return <Avatar className='players-data__avatar' src={record.ImagePath} />;
        }
        return <Avatar icon="user" />;
    }

    private setViewModel(data: Player[]) {
        if(data && Array.isArray(data) && data.length) {
            const players: TablePlayer[] = data.map((item) => ({
                key: item.Id,
                ImagePath: item.ImagePath,
                Name: item.Name,
                Points: item.Points,
                KdRatio: item.KdRatio,
                Kills: item.Kills,
                Deaths: item.Deaths,
                TotalGames: item.TotalGames
            }));
            return players;
        }  
    }

    private onRowClick(record: TablePlayer) {
        this.props.selectPlayer(record.key);
    }

    private cellWrapper(id: string, content: ReactNode) {
        const isSelectedClass = id === this.props.SelectedPlayer ? 'is-selected' : '';
        return <div className={`players-data__cell-inner ${isSelectedClass}`}>{content}</div>
    }

    onFormSubmit = (params: DateValues) => {
        this.fetchPlayers(this.props.playersDataUrl, params);
    }

    render() {
        const {IsLoading, DateFrom, DateTo, Players} = this.props;
        const { columns } = this.state;
        const players = this.setViewModel(Players);
        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <FilterForm 
                    onFormSubmit={this.onFormSubmit}
                    isLoading={IsLoading}
                    dateFrom={DateFrom}
                    dateTo={DateTo}
                />
                <Divider/>
                <Table
                    className="players-data"
                    rowClassName={() => "players-data__row"}
                    columns={columns}
                    dataSource={players}
                    pagination={false}
                    size="middle"
                    bordered={true}
                    scroll={{ x: true }}
                    loading={IsLoading}
                    onRow={(record) => {
                        return {
                            onClick: () => {
                                this.onRowClick(record);
                            }
                        };
                    }}
                    
                />
            </>
        );
    }
}
type PlayersDataProps = {
    playersDataUrl: string
    SelectedPlayer: string
    IsLoading: boolean
    DateFrom: string
    DateTo: string
    Players: Player[]
    fetchPlayers: typeof fetchPlayers
    startRequest: typeof startRequest
    selectPlayer: typeof selectPlayer
}

type TablePlayer = {
    key: string
    ImagePath: string
    Name: string
    Points: number
    KdRatio: number
    Kills: number
    Deaths: number
    TotalGames: number
}

const mapStateToProps = (state: AppState) => {
    const SelectedPlayer = state.SelectedPlayer;
    const IsLoading = state.IsLoading;
    const DateFrom = state.DateFrom;
    const DateTo = state.DateTo;
    const Players = state.Players;
    return { SelectedPlayer, IsLoading, DateFrom, DateTo, Players }
};

export default connect(mapStateToProps, { fetchPlayers, startRequest, selectPlayer })(PlayersData);
