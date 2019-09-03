import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import {
    Card, Icon, Avatar, Empty
} from 'antd';

const { Meta } = Card;
const PlayerCard = (props) => {
    if (props.PlayerData) {
        return (
            <Card
                cover={
                    <img
                        alt="example"
                        src="https://gw.alipayobjects.com/zos/rmsportal/JiqGstEfoWAOHiTxclqi.png"
                    />
                }
                actions={[
                    <Icon type="setting" key="setting" />,
                    <Icon type="edit" key="edit" />,
                    <Icon type="ellipsis" key="ellipsis" />,
                ]}
            >
                <Meta
                    avatar={<Avatar src="https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png" />}
                    title="Card title"
                    description="This is the description"
                />
            </Card>
        );
    }
    return <Empty/>;
};

PlayerCard.propTypes = {
    PlayerData: PropTypes.object.isRequired
};

export default PlayerCard;
