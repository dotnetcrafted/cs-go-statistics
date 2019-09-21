import React, {SFC} from 'react';
import {Card, Descriptions, Avatar, Empty, Divider, Typography} from 'antd';
import { connect } from 'react-redux';
import GunsChart from './guns-chart';
import Achievements from './achievements';
import {AppState} from "../../../general/js/redux/store";
import { IAppState, IPlayer, IGun } from '../../../general/js/redux/types';

const { Title } = Typography;
const { Meta } = Card;
const VISIBLE_GUNS = 5;
const PlayerCard: SFC<PlayerCardProps> = (props) => {
    if (props.store.SelectedPlayer) {
        const model = _getPlayerViewModel(props.store.SelectedPlayer, props.store.Players);
        const gunsToShow: IGun[] = model.Guns && [...model.Guns].slice(0, VISIBLE_GUNS);
        return (
            <Card
                className='player-card'
            >
                <Meta
                    className="player-card__meta"
                    avatar={renderAvatar(model.ImagePath)}
                    title={<Title level={2}>{model.Name}</Title>}
                    description={<Achievements data={model.Achievements} />}
                />
                <Divider orientation="left">Player's Statistics</Divider>
                <Descriptions>
                    <Descriptions.Item label="Kills">{model.Kills}</Descriptions.Item>
                    <Descriptions.Item label="Deaths">{model.Deaths}</Descriptions.Item>
                    <Descriptions.Item label="Assists">{model.Assists}</Descriptions.Item>
                    <Descriptions.Item label="HeadShot %">{model.HeadShot}</Descriptions.Item>
                    <Descriptions.Item label="Defused Bombs">{model.DefusedBombs}</Descriptions.Item>
                    <Descriptions.Item label="Exploded Bombs">{model.ExplodedBombs}</Descriptions.Item>
                    <Descriptions.Item label="Kd Ratio">{model.KdRatio}</Descriptions.Item>
                    <Descriptions.Item label="Kills Per Game">{model.KillsPerGame}</Descriptions.Item>
                    <Descriptions.Item label="Assists Per Game">{model.AssistsPerGame}</Descriptions.Item>
                    <Descriptions.Item label="Deaths Per Game">{model.DeathsPerGame}</Descriptions.Item>
                    <Descriptions.Item label="Friendly Kills">{model.FriendlyKills}</Descriptions.Item>
                    <Descriptions.Item label="Points">{model.Points}</Descriptions.Item>
                </Descriptions>
                <Divider orientation="left">{`Top ${VISIBLE_GUNS} Guns Used`}</Divider>
                {gunsToShow && <GunsChart guns={gunsToShow} />}
            </Card>
        );
    }
    return <Empty description="Choose a player from table"/>;
};

const renderAvatar = (src:  string) => {
    if (src) {
        return <Avatar size={48} shape="square" className='player-card__avatar' src={src} />;
    }
    return <Avatar size={48} shape="square" icon="user" />;
};


const _getPlayerViewModel = (id: string, data: IPlayer[]) => {
    const playersRow = data.filter((item) => item.Id === id)[0];
    return {
        Name: playersRow.Name,
        ImagePath: playersRow.ImagePath,
        Kills: playersRow.Kills,
        Deaths: playersRow.Deaths,
        Assists: playersRow.Assists,
        HeadShot: playersRow.HeadShot,
        DefusedBombs: playersRow.DefusedBombs,
        ExplodedBombs: playersRow.ExplodedBombs,
        KdRatio: playersRow.KdRatio,
        KillsPerGame: playersRow.KillsPerGame,
        AssistsPerGame: playersRow.AssistsPerGame,
        DeathsPerGame: playersRow.DeathsPerGame,
        FriendlyKills: playersRow.FriendlyKills,
        Guns: playersRow.Guns,
        Achievements: playersRow.Achievements,
        Points: playersRow.Points
    };
};

type PlayerCardProps = {
    store: IAppState
}

const mapStateToProps = (store: AppState) => store;
export default connect(mapStateToProps, { })(PlayerCard);
