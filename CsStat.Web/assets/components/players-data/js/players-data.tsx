import React, {ReactNode} from 'react';
import { Table, Avatar, Divider, Tooltip } from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers, startRequest, selectPlayer } from '../../../general/js/redux/actions';
import FilterForm, {IDateValues} from './filter-form';
import {AppState} from "../../../general/js/redux/store";
import { IAppState } from '../../../general/js/redux/types';

const CELL_CSS_CLASS = 'players-data__cell';

class PlayersData extends React.Component<PlayersDataProps> {
    readonly state = {
        PlayersData: [],
        columns: [
            {
                dataIndex: 'avatar',
                render: (link: any, record: TablePlayer) => {
                    const content = this._getAvatar(record);
                    return this._cellWrapper(record.key, content); 
                },
                width: '5%',
                className: CELL_CSS_CLASS
            },
            {
                title: 'Player Name',
                dataIndex: 'Name',
                className: CELL_CSS_CLASS,
                render: (link: any, record: TablePlayer) => {
                    return this._cellWrapper(record.key, record.Name); 
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
                render: (link: any, record: TablePlayer) => {
                    return this._cellWrapper(record.key, record.KdRatio); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.KdRatio - b.KdRatio,
            },
            {
                title: 'Kills',
                dataIndex: 'Kills',
                className: CELL_CSS_CLASS,
                render: (link: any, record: TablePlayer) => {
                    return this._cellWrapper(record.key, record.Kills); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.Kills - b.Kills,
            },
            {
                title: 'Deaths',
                dataIndex: 'Deaths',
                className: CELL_CSS_CLASS,
                render: (link: any, record: TablePlayer) => {
                    return this._cellWrapper(record.key, record.Deaths); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.Deaths - b.Deaths,
            },
            {
                title: 'Total Games',
                dataIndex: 'TotalGames',
                className: CELL_CSS_CLASS,
                render: (link: any, record: TablePlayer) => {
                    return this._cellWrapper(record.key, record.TotalGames); 
                },
                sorter: (a: TablePlayer, b: TablePlayer) => a.TotalGames - b.TotalGames,
            },
        ],
    };

    componentWillMount() {
        this._fetchPlayers(this.props.playersDataUrl);
    }

    _fetchPlayers(playersDataUrl: string, params?: IDateValues) {
        const url = new URL(playersDataUrl, window.location.origin);
        if (params) {
            url.search = new URLSearchParams(params).toString();
        }

        this.props.startRequest();

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: IAppState) => {
                this.props.fetchPlayers(data);
            });
        
    }

    onFormSubmit = (params: IDateValues) => {
        this._fetchPlayers(this.props.playersDataUrl, params);
    }

    _getAvatar(record: TablePlayer) {
        if (record.ImagePath) {
            return <Avatar className='players-data__avatar' src={record.ImagePath} />;
        }
        return <Avatar icon="user" />;
    }

    _setViewModel(data: IAppState) {
        if(data && Array.isArray(data) && data.length) {
            const players: TablePlayer[] = this.props.store.Players.map((item, i) => ({
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

    _onRowClick(record: TablePlayer) {
        this.props.selectPlayer(record.key);
    }

    _cellWrapper(id: string, content: ReactNode) {
        const isSelectedClass = id === this.props.store.SelectedPlayer ? 'is-selected' : '';
        return <div className={`players-data__cell-inner ${isSelectedClass}`}>{content}</div>
    }

    render() {
        const {IsLoading} = this.props.store;
        const { columns } = this.state;
        const players = this._setViewModel(this.props.store);
        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <FilterForm 
                    onFormSubmit={this.onFormSubmit}
                    isLoading={this.props.store.IsLoading}
                    dateFrom={this.props.store.DateFrom}
                    dateTo={this.props.store.DateTo}
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
                                this._onRowClick(record);
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
    store: IAppState,
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

const mapStateToProps = (store: AppState) => store;

export default connect(mapStateToProps, { fetchPlayers, startRequest, selectPlayer })(PlayersData);
