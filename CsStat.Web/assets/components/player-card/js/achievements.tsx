import React from 'react';
import { Popover } from 'antd';
import MapAchievementIdToImage from './mapping/achievements-image-map';

const Achievements = (props) => {
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


export default Achievements;