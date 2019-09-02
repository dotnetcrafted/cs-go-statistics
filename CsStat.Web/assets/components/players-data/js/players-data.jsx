import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import { Table, Avatar } from 'antd';

class PlayersData extends React.Component {
    constructor(props) {
        super(props);
        this.playersDataUrl = this.props.playersDataUrl;
        this.state = {
            columns: [
                {
                    dataIndex: 'avatar',
                    render: (link, record) => this.getAvatar(record),
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
        }
    }
    componentDidMount() {
        this.getPlayersData();
    }
    getPlayersData = () => {
        this.setState({isLoading: true})
        axios.get(this.playersDataUrl).then((response) => {
            this.setState({isLoading: false})
            this.setViewModel(response.data);
        }, (error) => {
            this.setState({isLoading: false})
            console.error(error);
        });
    }
    getAvatar(record) {
        if(record.avatar) {
            return <Avatar src={link} />
        } else {
            return <Avatar icon="user" />
        }
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
        this.setState({playersData})
    }
    render() {
        const {isLoading, columns, playersData} = this.state;
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
    playersDataUrl: PropTypes.string.isRequired
};

export default PlayersData;