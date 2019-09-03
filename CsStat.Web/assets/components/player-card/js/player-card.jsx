import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import {
    Card, Descriptions, Avatar, Empty, Divider
} from 'antd';
import { connect } from 'react-redux';

const { Meta } = Card;
const PlayerCard = (props) => {
    if (props.selectedPlayer) {
        const model = _getPlayerViewModel(props.selectedPlayer, props.playersData);
        return (
            <Card
                cover={getCover(model.ImagePath)}
            >
                <Meta
                    avatar={getAvatar(model.ImagePath)}
                    title={model.NickName}
                    description={model.FullName}
                />
                <Divider orientation="left">Player's Statistics</Divider>
                <Descriptions>
                    <Descriptions.Item label="Kills">{model.Kills}</Descriptions.Item>
                    <Descriptions.Item label="Death">{model.Death}</Descriptions.Item>
                    <Descriptions.Item label="Assists">{model.Assists}</Descriptions.Item>
                    <Descriptions.Item label="HeadShot">{model.HeadShot}</Descriptions.Item>
                    <Descriptions.Item label="Total Games">{model.TotalGames}</Descriptions.Item>
                    <Descriptions.Item label="Defuse">{model.Defuse}</Descriptions.Item>
                    <Descriptions.Item label="Explode">{model.Explode}</Descriptions.Item>
                    <Descriptions.Item label="Favorite Gun">{model.FavoriteGun}</Descriptions.Item>
                    <Descriptions.Item label="Kd Ratio">{model.KdRatio}</Descriptions.Item>
                    <Descriptions.Item label="Kills Per Game">{model.KillsPerGame}</Descriptions.Item>
                    <Descriptions.Item label="Assists Per Game">{model.AssistsPerGame}</Descriptions.Item>
                    <Descriptions.Item label="Death Per Game">{model.DeathPerGame}</Descriptions.Item>
                </Descriptions>

            </Card>
        );
    }
    return <Empty/>;
};


const getCover = (src) => {
    if (src) {
        return <img alt={model.NickName} src={model.ImagePath}/>;
    }
    return false;
};

const getAvatar = (src) => {
    if (src) {
        return <Avatar src={src} />;
    }
    return <Avatar icon="user" />;
};

const _getPlayerViewModel = (id, data) => {
    const playersRow = data.filter((item) => item.Player.Id === id)[0];
    return {
        NickName: playersRow.Player.NickName,
        FullName: playersRow.Player.FullName,
        ImagePath: playersRow.Player.ImagePath,
        Kills: playersRow.Kills,
        Death: playersRow.Death,
        Assists: playersRow.Assists,
        HeadShot: playersRow.HeadShot,
        TotalGames: playersRow.TotalGames,
        Defuse: playersRow.Defuse,
        Explode: playersRow.Explode,
        FavoriteGun: playersRow.FavoriteGun,
        KdRatio: playersRow.KdRatio,
        KillsPerGame: playersRow.KillsPerGame,
        AssistsPerGame: playersRow.AssistsPerGame,
        DeathPerGame: playersRow.DeathPerGame
    };
};


PlayerCard.propTypes = {
    playersData: PropTypes.object.isRequired,
    selectedPlayer: PropTypes.string.isRequired
};

const mapStateToProps = (state) => {
    const playersData = state.players.allPlayers;
    const selectedPlayer = state.players.selectedPlayer;
    return { playersData, selectedPlayer };
};
export default connect(mapStateToProps, { })(PlayerCard);
