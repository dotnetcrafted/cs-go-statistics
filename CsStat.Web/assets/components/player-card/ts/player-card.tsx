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
import { getPlayerById, DEFAULT_CMS_PLAYER } from 'project/helpers';

const { Title } = Typography;
const { Meta } = Card;
const VISIBLE_GUNS = 5;
const PlayerCard: SFC<PlayerCardProps> = (props) => {
    const search = qs.parse(props.router.location.search);
    const { playerId } = search;

    const onRelatedPlayerSelect = (name: string) => {
        const id = getIdByName(name, props.players);

        if (!id) {
            throw new Error('No players found with this Name');
        }
        const search = utils.getUrlSearch({ playerId: id }, props.router.location.search);
        history.push({
            search
        });
    };

    if (typeof playerId === 'string' && props.players.length > 0) {
        const model = getPlayerViewModel(playerId, props.players);
        
        if (model) {
            const gunsToShow: Gun[] = model.guns && [...model.guns].slice(0, VISIBLE_GUNS);
            const cmsPlayer = getPlayerById(model.steamId) || DEFAULT_CMS_PLAYER
            
            return (
                <Card className="player-card">
                    <Meta
                        className="player-card__meta"
                        avatar={renderAvatar(cmsPlayer.steamImage)}
                        title={<Title level={2}>{cmsPlayer.nickName}</Title>}
                        description={<Achievements data={model.achievements} />}
                    />
                    <Divider orientation="left">Player's Statistics</Divider>
                    <Descriptions>
                        <Descriptions.Item label="Kills">{model.kills}</Descriptions.Item>
                        <Descriptions.Item label="Deaths">{model.deaths}</Descriptions.Item>
                        <Descriptions.Item label="Assists">{model.assists}</Descriptions.Item>
                        <Descriptions.Item label="HeadShots">{utils.getHeadshotsString(model.headShot, model.kills)}</Descriptions.Item>
                        <Descriptions.Item label="Defused Bombs">{model.defusedBombs}</Descriptions.Item>
                        <Descriptions.Item label="Exploded Bombs">{model.explodedBombs}</Descriptions.Item>
                        <Descriptions.Item label="Kd Ratio">{model.kdRatio}</Descriptions.Item>
                        <Descriptions.Item label="Kills Per Game">{model.killsPerGame}</Descriptions.Item>
                        <Descriptions.Item label="Assists Per Game">{model.assistsPerGame}</Descriptions.Item>
                        <Descriptions.Item label="Deaths Per Game">{model.deathsPerGame}</Descriptions.Item>
                        <Descriptions.Item label="Friendly Kills">{model.friendlyKills}</Descriptions.Item>
                        <Descriptions.Item label="Points">{model.points}</Descriptions.Item>
                    </Descriptions>
                    <Divider orientation="left">{`Top ${VISIBLE_GUNS} Guns Used`}</Divider>
                    {gunsToShow && <GunsChart guns={gunsToShow} />}
                    <Divider orientation="left">{'Victims'}</Divider>
                    <RelatedPlayers data={model.victims} onRelatedPlayerSelect={onRelatedPlayerSelect} killerType={true} />
                    <Divider orientation="left">{`Killers`}</Divider>
                    <RelatedPlayers data={model.killers} onRelatedPlayerSelect={onRelatedPlayerSelect} killerType={false} />
                </Card>
            );
        }
    }
    return <Empty description="Choose a player from table" />;
};

const getIdByName = (name: string, players: Player[]): string | undefined => {
    const player = players.find((player) => player.name === name);
    if (player) {
        return player.id;
    }
};
const renderAvatar = (src: string) => {
    if (src) {
        return <Avatar size={48} shape="square" className="player-card__avatar" src={src} />;
    }
    return <Avatar size={48} shape="square" icon="user" />;
};
const getPlayerViewModel = (id: string, data: Player[]) => {
    const playersRow = data.filter((item) => item.id === id)[0];
    let model = null;

    if (playersRow) {
        model = {
            id: playersRow.id,
            steamId: playersRow.steamId,
            kills: playersRow.kills,
            deaths: playersRow.deaths,
            assists: playersRow.assists,
            headShot: playersRow.headShot,
            defusedBombs: playersRow.defusedBombs,
            explodedBombs: playersRow.explodedBombs,
            kdRatio: playersRow.kdRatio,
            killsPerGame: playersRow.killsPerGame,
            assistsPerGame: playersRow.assistsPerGame,
            deathsPerGame: playersRow.deathsPerGame,
            friendlyKills: playersRow.friendlyKills,
            guns: playersRow.guns,
            achievements: playersRow.achievements,
            points: playersRow.points,
            victims: playersRow.victims,
            killers: playersRow.killers
        };
    }

    return model;
};

type PlayerCardProps = {
    router: any;
    players: Player[];
};

const mapStateToProps = (state: RootState) => {
    const players = state.app.players;
    const router = state.router;
    return { players, router };
};
export default connect(
    mapStateToProps
)(PlayerCard);
