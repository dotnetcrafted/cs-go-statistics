import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import {
    Layout, Icon, Typography, Row, Col
} from 'antd';

const { Header, Content } = Layout;
const { Title } = Typography;

const HomePage = (props) => (
    <Layout className="home-page__layout">
        <Header className="home-page__header">
            <Row type="flex" justify="start" align="middle">
                <Col xs={6} lg={1} className="home-page__logo"><Icon type="database" theme="filled" /></Col>
                <Col xs={18} lg={6}><Title className="home-page__title">Fuse8 CS:GO Statistics</Title></Col>
            </Row>
        </Header>
        <Content className="home-page__content">
            <Row type="flex" justify="start" align="middle">
                <Col xs={24} lg={12}>
                    {props.children}

                </Col>
            </Row>
        </Content>
    </Layout>
);

HomePage.propTypes = {
    children: PropTypes.node.isRequired
};

export default HomePage;
