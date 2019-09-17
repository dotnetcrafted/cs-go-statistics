import React, {SFC, ReactNode} from 'react';
import { Provider } from 'react-redux';
import {
    Layout, Icon, Typography, Row, Col
} from 'antd';
import store from '../../../general/js/redux-store';

const { Header, Content, Footer } = Layout;
const { Title } = Typography;

type Props = { 
    playersData: ReactNode,
    playerCard: ReactNode 
}

const HomePage: SFC<Props> = (props) => (
    <Provider store={store}>
        <Layout className="home-page__layout">
            <Header className="home-page__header">
                <Row type="flex" justify="start" align="middle">
                    <Col xs={6} lg={1} className="home-page__logo"><Icon type="database" theme="filled" /></Col>
                    <Col xs={18} lg={6}><Title className="home-page__title">Fuse8 CS:GO Statistics</Title></Col>
                </Row>
            </Header>
            <Content className="home-page__content">
                <Row type="flex" justify="start">
                    <Col xs={24} lg={12}>
                        {props.playersData}
                    </Col>
                    <Col xs={24} lg={{ span: 11, offset: 1 }}>
                        <div className="home-page__card">{props.playerCard}</div>
                    </Col>
                </Row>
            </Content>
            <Footer><div>Icons made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div></Footer>
        </Layout>
    </Provider>
);
export default HomePage;
