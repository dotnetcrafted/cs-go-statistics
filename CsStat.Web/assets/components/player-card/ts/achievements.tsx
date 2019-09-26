import React, {SFC} from 'react';
import { Popover } from 'antd';
import MapAchievementIdToImage from './mapping/achievements-image-map';
import { Achievement } from '../../../general/ts/redux/types';
const Achievements: SFC<AchievementsProps> = (props) => {
    const { data } = props;
    return (
        <div className="achievements">
            {data && data.map((item) => (
                <div className="achievements__item" key={item.Id}>
                    <Popover title={item.Name} content={item.Description}>
                        <img className="achievements__icon" src={MapAchievementIdToImage(item.Id)} />
                    </Popover>
                </div>
            ))}
        </div>
    );
};

type AchievementsProps = {
    data: Achievement[]
}

export default Achievements;
