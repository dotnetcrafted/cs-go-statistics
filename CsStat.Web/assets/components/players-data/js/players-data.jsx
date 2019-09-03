import React from 'react';
import PropTypes from 'prop-types';
import { Table, Avatar } from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers } from '../../../general/js/redux-actions';

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
                    dataIndex: 'NickName',
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
                    title: 'Death',
                    dataIndex: 'Death',
                }
            ],
            playersData: [],
            isLoading: false
        };
    }

    componentWillMount() {
        this.props.fetchPlayers();
    }


    getAvatar(link, record) {
        if (record.avatar) {
            return <Avatar src={link} />;
        }
        return <Avatar icon="user" />;
    }

    setViewModel(data) {
        const playersData = data.map((item, i) => ({
            key: i,
            avatar: item.Player.ImagePath,
            NickName: item.Player.NickName,
            KdRatio: item.KdRatio,
            Kills: item.Kills,
            Death: item.Death
        }));
        return playersData;
    }

    render() {
        const { isLoading, columns } = this.state;
        const playersData = this.setViewModel(this.props.playersData);
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
            />
        );
    }
}

PlayersData.propTypes = {
    playersDataUrl: PropTypes.string.isRequired,
    playersData: PropTypes.object.isRequired,
    fetchPlayers: PropTypes.func.isRequired
};

const mapStateToProps = (state) => ({
    playersData: state.items.items
});
export default connect(mapStateToProps, { fetchPlayers })(PlayersData);
