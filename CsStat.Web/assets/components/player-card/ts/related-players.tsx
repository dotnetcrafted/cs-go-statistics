import React, { SFC } from 'react';
import { Tooltip, Avatar, Badge } from 'antd';
import { RelatedPlayer } from '../../../general/ts/redux/types';
const RelatedPlayers: SFC<RelatedPlayersProps> = props => {
    const { data, killerType } = props;
    const theme = (killerType: boolean) => {
        const backgroundColor = killerType ? '#f50' : '#87d068';
        return { backgroundColor };
    };
    return (
        <div className="related-players">
            {data &&
                data.map(item => (
                    <div className="related-players__item">
                        <Tooltip title={item.Name}>
                            <Badge count={item.Count} style={theme(killerType)}>
                                <Avatar
                                    size={48}
                                    shape="square"
                                    className="related-players__avatar"
                                    src={item.ImagePath}
                                />
                            </Badge>
                        </Tooltip>
                    </div>
                ))}
        </div>
    );
};

type RelatedPlayersProps = {
    data: RelatedPlayer[];
    killerType: boolean;
};

export default RelatedPlayers;
