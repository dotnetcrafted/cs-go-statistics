import React, { SFC, ReactNode } from 'react';
import {
    Layout, Typography, Row, Col, Divider
} from 'antd';
import SnowStorm from 'react-snowstorm';
import SurpriseSanta from 'surprise-santa';
import IconCopyright from '../../icon-copyright/ts/icon-copyright';
import AuthorsCopyright from '../../authors-copyright/ts/authors-copyright';
import '../scss/index.scss';

const { Header, Content, Footer } = Layout;
const { Title } = Typography;

const BaseLayout: SFC<BaseLayoutProps> = (props) => (
    <Layout className="base-layout__layout">
        <Header className="base-layout__header">
            <Row type="flex" justify="start" align="middle">
                <Col xs={6} lg={1} className="base-layout__logo">
                    <span style={{ fontSize: '3rem', }}>🎄</span>
                </Col>
                <Col xs={18} lg={6}>
                    <Title level={1} className="base-layout__title">
                        Fuse8 CS:GO Statistics
                    </Title>
                </Col>
            </Row>
        </Header>
        <Content className="base-layout__content">
            {props.children}
        </Content>
        <Footer>
            <AuthorsCopyright />
            <Divider />
            <a href="https://bitbucket.org/radik_fayskhanov/counterstrikestat">Repository is available on Bitbucket.</a>
            <Divider />
            <IconCopyright />
        </Footer>
        <SnowStorm snowColor="#00CCFF" />
        <SurpriseSanta minTime={40} maxTime={100} />
    </Layout>
);

type BaseLayoutProps = {
    children: ReactNode;
};

export default BaseLayout;
