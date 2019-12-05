import React, { SFC } from 'react';
import { Tooltip, Avatar, Badge } from 'antd';
import { Victim } from '../../../general/ts/redux/types';
const Victims: SFC<VictimsProps> = props => {
    const { data } = props;
    return (
        <div className="victims">
            {data &&
                data.map(item => (
                    <div className="victims__item">
                        <Tooltip title={item.Name}>
                            <Badge count={item.Deaths}>
                                <Avatar size={48} shape="square" className="victim__avatar" src={item.ImagePath} />
                            </Badge>
                        </Tooltip>
                    </div>
                ))}
        </div>
    );
};

type VictimsProps = {
    data: Victim[];
};

export default Victims;
