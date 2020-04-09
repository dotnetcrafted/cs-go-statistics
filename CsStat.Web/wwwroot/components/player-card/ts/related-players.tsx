import React, { SFC } from 'react';
import { Tooltip, Avatar, Badge } from 'antd';
import { RelatedPlayer } from 'general/ts/redux/types';
import { getPlayerById, DEFAULT_CMS_PLAYER } from 'project/helpers';

const RelatedPlayers: SFC<RelatedPlayersProps> = (props) => {
    const { data, killerType } = props;
    const theme = (killerType: boolean) => {
        const backgroundColor = killerType ? '#4474d5' : '#cb4848';
        return { backgroundColor };
    };

    const onPlayerSelect = function (name: string) {
        props.onRelatedPlayerSelect(name);
    };

    return (
        <div className="related-players">
            {data &&
                data.map((item) => {
                    const cmsPlayer = getPlayerById(item.steamId) || DEFAULT_CMS_PLAYER;
                    
                    return (
                        <div key={cmsPlayer.nickName} className="related-players__item" onClick={() => onPlayerSelect(cmsPlayer.nickName)}>
                            <Tooltip title={cmsPlayer.nickName}>
                                <Badge count={item.count} overflowCount={9999} style={theme(killerType)}>
                                    <Avatar
                                        size={48}
                                        shape="square"
                                        className="related-players__avatar"
                                        src={cmsPlayer.steamImage}
                                    />
                                </Badge>
                            </Tooltip>
                        </div>
                    )
                })
            }
        </div>
    );
};

type RelatedPlayersProps = {
    data: RelatedPlayer[];
    killerType: boolean;
    onRelatedPlayerSelect: (playerId: string) => void;
};

export default RelatedPlayers;
