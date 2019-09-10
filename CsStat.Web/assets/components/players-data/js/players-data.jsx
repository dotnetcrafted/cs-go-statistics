import React from 'react';
import PropTypes from 'prop-types';
import {
    Table, Avatar, Divider, Tooltip
} from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers, selectPlayer } from '../../../general/js/redux-actions';
import FilterForm from './filter-form';

class PlayersData extends React.Component {
    constructor(props) {
        super(props);
        this.playersDataUrl = this.props.playersDataUrl;
        this.state = {
            columns: [
                {
                    dataIndex: 'avatar',
                    render: (link, record) => this._getAvatar(link, record),
                    width: '5%',
                },
                {
                    title: 'Player Name',
                    dataIndex: 'Name',
                },
                {
                    title: 'Kills',
                    dataIndex: 'Kills',
                },
                {
                    title: (data)=>(
                        <Tooltip title="Kills / Deaths">
                            K/D Ratio
                        </Tooltip>
                    ),
                    dataIndex: 'KdRatio',
                },
                {
                    title: (data)=>(
                        <Tooltip title="Kills + Assists + (Defuses + Exploded Bombs) * 5 - Friendly Kills * 2">
                            Points
                        </Tooltip>
                    ),
                    dataIndex: 'Points',
                },
                {
                    title: 'Deaths',
                    dataIndex: 'Deaths',
                }
            ],
            playersData: []
        };
    }

    componentWillMount() {
        this._fetchPlayers(this.props.playersDataUrl);
    }

    _onRowButtonClick =(id)=> {
        this.props.selectPlayer(id);
    }

    _fetchPlayers(url, params) {
        this.props.fetchPlayers(url, params || {});
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

    _setViewModel() {
        const playersData = this.props.playersData.map((item, i) => ({
            key: item.Id,
            ImagePath: item.ImagePath,
            Name: item.Name,
            Points: item.Points,
            KdRatio: item.KdRatio,
            Kills: item.Kills,
            Deaths: item.Deaths
        }));
        return playersData;
    }

    _onRowClick(record, rowIndex) {
        console.log(record, rowIndex);
        this.props.selectPlayer(record.key);
    }

    render() {
        const {isLoading} = this.props;
        const { columns } = this.state;
        const playersData = this._setViewModel();
        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <FilterForm onFormSubmit={this.onFormSubmit} isLoading={this.props.isLoading} />
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

PlayersData.propTypes = {
    playersDataUrl: PropTypes.string.isRequired,
    playersData: PropTypes.object.isRequired,
    fetchPlayers: PropTypes.func.isRequired,
    selectedPlayer: PropTypes.string.isRequired,
    isLoading: PropTypes.bool.isRequired
};

const mapStateToProps = (state) => {
    const playersData = state.players.allPlayers;
    const selectedPlayer = state.players.selectedPlayer;
    const isLoading = state.players.isLoading;
    return { playersData, selectedPlayer, isLoading }
};
export default connect(mapStateToProps, { fetchPlayers, selectPlayer })(PlayersData);
