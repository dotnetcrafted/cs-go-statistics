import React, { SFC, ReactNode } from 'react';
import { Provider } from 'react-redux';
import { Layout, Icon, Typography, Row, Col, Divider } from 'antd';
import configureStore from '../../../general/ts/redux/store';
import IconCopyright from './icon-copyright';
import AuthorsCopyright from './authors-copyright';

const store = configureStore();
const { Header, Content, Footer } = Layout;
const { Title } = Typography;

const HomePage: SFC<HomePageProps> = props => (
    <Provider store={store}>
        <Layout className="home-page__layout">
            <Header className="home-page__header">
                <Row type="flex" justify="start" align="middle">
                    <Col xs={6} lg={1} className="home-page__logo">
                        <Icon type="database" theme="filled" />
                    </Col>
                    <Col xs={18} lg={6}>
                        <Title className="home-page__title">Fuse8 CS:GO Statistics</Title>
                    </Col>
                </Row>
            </Header>
            <Content className="home-page__content">
                <Row type="flex" justify="start">
                    <Col xs={24} lg={14}>
                        {props.playersData}
                    </Col>
                    <Col xs={24} lg={{ span: 9, offset: 1 }}>
                        <div className="home-page__card">{props.playerCard}</div>
                    </Col>
                </Row>
            </Content>
            <Footer>
                <AuthorsCopyright />
                <Divider />
                <a href="https://bitbucket.org/radik_fayskhanov/counterstrikestat">
                    Repository is available on Bitbucket.
                </a>
                <Divider />
                <IconCopyright />
            </Footer>
        </Layout>
    </Provider>
);

type HomePageProps = {
    playersData: ReactNode;
    playerCard: ReactNode;
};

export default HomePage;
