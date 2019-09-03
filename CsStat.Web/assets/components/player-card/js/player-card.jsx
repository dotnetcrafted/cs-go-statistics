import React from 'react';
import PropTypes from 'prop-types';
import {
    Card, Descriptions, Avatar, Empty, Divider
} from 'antd';
import { connect } from 'react-redux';
import GunsChart from './guns-chart';

const { Meta } = Card;
const PlayerCard = (props) => {
    if (props.selectedPlayer) {
        const model = _getPlayerViewModel(props.selectedPlayer, props.playersData);
        return (
            <Card
                className='player-card'
                cover={renderCover(model)}
            >
                <Meta
                    avatar={renderAvatar(model.ImagePath)}
                    title={model.Name}
                />
                <Divider orientation="left">Player's Statistics</Divider>
                <Descriptions>
                    <Descriptions.Item label="Kills">{model.Kills}</Descriptions.Item>
                    <Descriptions.Item label="Deaths">{model.Deaths}</Descriptions.Item>
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
                <Divider orientation="left">Guns usage chart</Divider>
                { model.Guns && <GunsChart guns={model.Guns} />}
            </Card>
        );
    }
    return <Empty/>;
};
const renderCover = (model) => {
    if (model.ImagePath) {
        return <div className='player-card__cover-wrapper'><img alt={model.Name} src='https://i.imgur.com/69Ig9Mi.jpg'/></div>;
    }
    return false;
};

const renderAvatar = (src) => {
    if (src) {
        return <Avatar className='player-card__avatar' src='https://i.imgur.com/69Ig9Mi.jpg' />;
    }
    return <Avatar icon="user" />;
};

const _getPlayerViewModel = (id, data) => {
    const playersRow = data.filter((item) => item.Id === id)[0];
    return {
        Name: playersRow.Name,
        ImagePath: playersRow.ImagePath,
        Kills: playersRow.Kills,
        Deaths: playersRow.Deaths,
        Assists: playersRow.Assists,
        HeadShot: playersRow.HeadShot,
        TotalGames: playersRow.TotalGames,
        Defuse: playersRow.Defuse,
        Explode: playersRow.Explode,
        FavoriteGun: playersRow.FavoriteGun,
        KdRatio: playersRow.KdRatio,
        KillsPerGame: playersRow.KillsPerGame,
        AssistsPerGame: playersRow.AssistsPerGame,
        DeathPerGame: playersRow.DeathPerGame,
        Guns: playersRow.Guns
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
