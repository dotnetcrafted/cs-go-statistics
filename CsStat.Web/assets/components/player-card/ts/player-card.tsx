import React, { SFC } from 'react';
import {
    Card, Descriptions, Avatar, Empty, Divider, Typography
} from 'antd';
import { connect } from 'react-redux';
import qs from 'query-string';
import GunsChart from './guns-chart';
import Achievements from './achievements';
import RelatedPlayers from './related-players';
import { RootState, Player, Gun } from '../../../general/ts/redux/types';
import '../scss/index.scss';
import utils from '../../../general/ts/utils';
import { history } from '../../../general/ts/redux/store';

const { Title } = Typography;
const { Meta } = Card;
const VISIBLE_GUNS = 5;
const PlayerCard: SFC<PlayerCardProps> = (props) => {
    const search = qs.parse(props.router.location.search);
    const { PlayerId } = search;

    const onRelatedPlayerSelect = (name: string) => {
        const id = getIdByName(name, props.Players);
        if (!id) {
            throw new Error('No players found with this Name');
        }
        const search = utils.getUrlSearch({ PlayerId: id }, props.router.location.search);
        history.push({
            search
        });
    };

    if (typeof PlayerId === 'string' && props.Players.length > 0) {
        const model = _getPlayerViewModel(PlayerId, props.Players);
        const gunsToShow: Gun[] = model.Guns && [...model.Guns].slice(0, VISIBLE_GUNS);
        return (
            <Card className="player-card">
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
                    <Descriptions.Item label="HeadShots">{utils.getHeadshotsString(model.HeadShot, model.Kills)}</Descriptions.Item>
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
                <Divider orientation="left">{`${model.Name} has killed them:`}</Divider>
                <RelatedPlayers data={model.Victims} onRelatedPlayerSelect={onRelatedPlayerSelect} killerType={false} />
                <Divider orientation="left">{`They have killed ${model.Name}`}</Divider>
                <RelatedPlayers data={model.Killers} onRelatedPlayerSelect={onRelatedPlayerSelect} killerType={true} />
            </Card>
        );
    }
    return <Empty description="Choose a player from table" />;
};

const getIdByName = (name: string, players: Player[]): string | undefined => {
    const player = players.find((player) => player.Name === name);
    if (player) {
        return player.Id;
    }
};
const renderAvatar = (src: string) => {
    if (src) {
        return <Avatar size={48} shape="square" className="player-card__avatar" src={src} />;
    }
    return <Avatar size={48} shape="square" icon="user" />;
};
const _getPlayerViewModel = (id: string, data: Player[]) => {
    const playersRow = data.filter((item) => item.Id === id)[0];
    return {
        Id: playersRow.Id,
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
        Points: playersRow.Points,
        Victims: playersRow.Victims,
        Killers: playersRow.Killers
    };
};
type PlayerCardProps = {
    router: any;
    Players: Player[];
};

const mapStateToProps = (state: RootState) => {
    const Players = state.app.Players;
    const router = state.router;
    return { Players, router };
};
export default connect(
    mapStateToProps
)(PlayerCard);
