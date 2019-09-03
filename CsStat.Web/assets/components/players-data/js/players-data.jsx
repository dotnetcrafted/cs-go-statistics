import React from 'react';
import PropTypes from 'prop-types';
import {
    Table, Avatar, Button, Icon
} from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers, selectPlayer } from '../../../general/js/redux-actions';

class PlayersData extends React.Component {
    constructor(props) {
        super(props);
        this.playersDataUrl = this.props.playersDataUrl;
        this.state = {
            columns: [
                {
                    dataIndex: 'avatar',
                    render: (link, record) => this.getAvatar(link, record),
                    width: '5%',
                },
                {
                    title: 'Player Name',
                    dataIndex: 'Name',
                },
                {
                    title: 'K/D Ratio',
                    dataIndex: 'KdRatio',
                },
                {
                    title: 'Kills',
                    dataIndex: 'Kills',
                },
                {
                    title: 'Deaths',
                    dataIndex: 'Deaths',
                },
                {
                    title: 'Detailed Info',
                    key: 'Button',
                    fixed: 'right',
                    width: 100,
                    render: (data) => this.renderButton(data),
                }
            ],
            playersData: [],
            isLoading: false
        };
    }

    componentWillMount() {
        this.props.fetchPlayers(this.props.playersDataUrl);
    }

    renderButton = (data) => {
        return (
            <Button type="dashed" onClick={()=>this.onRowButtonClick(data.Button)}>
                Show more
                <Icon type='right' />
            </Button>
        )
    }

    onRowButtonClick =(id)=> {
        this.props.selectPlayer(id);
    }


    getAvatar(link, record) {
        if (record.avatar) {
            return <Avatar className='players-data__avatar' src='https://i.imgur.com/69Ig9Mi.jpg' />;
        }
        return <Avatar icon="user" />;
    }

    setViewModel() {
        const playersData = this.props.playersData.map((item, i) => ({
            key: i,
            avatar: item.ImagePath,
            Name: item.Name,
            KdRatio: item.KdRatio,
            Kills: item.Kills,
            Deaths: item.Deaths,
            Button: item.Id
        }));
        return playersData;
    }

    render() {
        const { isLoading, columns } = this.state;
        const playersData = this.setViewModel();
        return (
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
            />
        );
    }
}

PlayersData.propTypes = {
    playersDataUrl: PropTypes.string.isRequired,
    playersData: PropTypes.object.isRequired,
    fetchPlayers: PropTypes.func.isRequired,
    selectedPlayer: PropTypes.string.isRequired
};

const mapStateToProps = (state) => {
    const playersData = state.players.allPlayers;
    const selectedPlayer = state.players.selectedPlayer;
    return { playersData, selectedPlayer }
};
export default connect(mapStateToProps, { fetchPlayers, selectPlayer })(PlayersData);
