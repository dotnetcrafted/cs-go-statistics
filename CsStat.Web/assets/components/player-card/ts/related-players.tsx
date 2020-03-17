import React, { SFC } from 'react';
import { Tooltip, Avatar, Badge } from 'antd';
import { RelatedPlayer } from '../../../general/ts/redux/types';

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
                data.map((item) => (
                    <div key={item.Name} className="related-players__item" onClick={() => onPlayerSelect(item.Name)}>
                        <Tooltip title={item.Name}>
                            <Badge count={item.Count} overflowCount={9999} style={theme(killerType)}>
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
    onRelatedPlayerSelect: (playerId: string) => void;
};

export default RelatedPlayers;
