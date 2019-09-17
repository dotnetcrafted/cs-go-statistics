import React from 'react';
import {
    Table, Avatar, Divider, Tooltip
} from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers, startRequest, selectPlayer } from '../../../general/js/redux-actions';
import FilterForm from './filter-form';
import {AppState} from "../../../general/js/redux/store";

const CELL_CSS_CLASS = 'players-data__cell';
const initialState = {
    playersData = [],
    columns: [
        {
            dataIndex: 'avatar',
            render: (link, record) => {
                const content = this._getAvatar(link, record);
                return this._cellWrapper(record.key, content); 
            },
            width: '5%',
            className: CELL_CSS_CLASS
        },
        {
            title: 'Player Name',
            dataIndex: 'Name',
            className: CELL_CSS_CLASS,
            render: (link, record) => {
                return this._cellWrapper(record.key, record.Name); 
            },
            sorter: (a, b) => a.Name.localeCompare(b.Name),
        },
        {
            title: (data)=>(
                <Tooltip title="Kills / Deaths">
                    K/D Ratio
                </Tooltip>
            ),
            dataIndex: 'KdRatio',
            className: CELL_CSS_CLASS,
            render: (link, record) => {
                return this._cellWrapper(record.key, record.KdRatio); 
            },
            sorter: (a, b) => a.KdRatio - b.KdRatio,
        },
        {
            title: 'Kills',
            dataIndex: 'Kills',
            className: CELL_CSS_CLASS,
            render: (link, record) => {
                return this._cellWrapper(record.key, record.Kills); 
            },
            sorter: (a, b) => a.Kills - b.Kills,
        },
        {
            title: 'Deaths',
            dataIndex: 'Deaths',
            className: CELL_CSS_CLASS,
            render: (link, record) => {
                return this._cellWrapper(record.key, record.Deaths); 
            },
            sorter: (a, b) => a.Deaths - b.Deaths,
        },
        {
            title: 'Total Games',
            dataIndex: 'TotalGames',
            className: CELL_CSS_CLASS,
            render: (link, record) => {
                return this._cellWrapper(record.key, record.TotalGames); 
            },
            sorter: (a, b) => a.TotalGames - b.TotalGames,
        },
    ],
};
type State = Readonly<typeof initialState>;

class PlayersData extends React.Component<object, State> {
    readonly state:State = initialState;

    componentWillMount() {
        this._fetchPlayers(this.props.playersDataUrl);
    }

    _onRowButtonClick =(id)=> {
        this.props.selectPlayer(id);
    }

    _fetchPlayers(playersDataUrl: string, params) {
        const url = new URL(playersDataUrl, window.location.origin);
        if (params) {
            url.search = new URLSearchParams(params);
        }

        this.props.selectPlayer();

        fetch(url)
            .then((res) => res.json())
            .then((data) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;
                this.props.fetchPlayers(data);
            });
        
    }

    onFormSubmit = (params) => {
        this._fetchPlayers(this.props.playersDataUrl, params);
    }

    _getAvatar(link, record) {
        if (record.ImagePath) {
            return <Avatar className='players-data__avatar' src={record.ImagePath} />;
        }
        return <Avatar icon="user" />;
    }

    _setViewModel(data) {
        if(data && Array.isArray(data) && data.length) {
            const playersData = this.props.playersData.map((item, i) => ({
                key: item.Id,
                ImagePath: item.ImagePath,
                Name: item.Name,
                Points: item.Points,
                KdRatio: item.KdRatio,
                Kills: item.Kills,
                Deaths: item.Deaths,
                TotalGames: item.TotalGames
            }));
            return playersData;
        }        
    }

    _onRowClick(record) {
        this.props.selectPlayer(record.key);
    }

    _cellWrapper(id, content) {
        const isSelectedClass = id === this.props.selectedPlayer ? 'is-selected' : '';
        return <div className={`players-data__cell-inner ${isSelectedClass}`}>{content}</div>
    }

    render() {
        const {isLoading} = this.props;
        const { columns } = this.state;
        const playersData = this._setViewModel(this.props.playersData);
        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <FilterForm 
                    onFormSubmit={this.onFormSubmit}
                    isLoading={this.props.isLoading}
                    dateFrom={this.props.dateFrom}
                    dateTo={this.props.dateTo}
                />
                <Divider/>
                <Table
                    className="players-data"
                    rowClassName="players-data__row"
                    columns={columns}
                    dataSource={playersData}
                    pagination={false}
                    loading={isLoading}
                    size="middle"
                    bordered={true}
                    scroll={{ x: true }}
                    loading={isLoading}
                    onRow={(record, rowIndex) => {
                        return {
                        onClick: () => {
                            this._onRowClick(record, rowIndex);
                        }
                        };
                    }}
                    
                />
            </>
        );
    }
}

const mapStateToProps = (state: AppState) => {
    const playersData = state.players;
    const selectedPlayer = state.selectedPlayer;
    const isLoading = state.isLoading;
    const dateFrom = state.DateFrom;
    const dateTo = state.DateTo;
    return { playersData, selectedPlayer, isLoading, dateFrom, dateTo }
};
export default connect(mapStateToProps, { fetchPlayers, startRequest, selectPlayer })(PlayersData);
