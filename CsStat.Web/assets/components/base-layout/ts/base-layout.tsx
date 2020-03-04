import React, { SFC, ReactNode } from 'react';
import { Link } from 'react-router-dom';
import {
    Layout, Typography, Row, Col
} from 'antd';
import IconCopyright from '../../icon-copyright';
import AuthorsCopyright from '../../authors-copyright';
import Repository from '../../repository';
import '../scss/index.scss';
import Navigation from '../../navigation';
import { ServerInfo } from 'components/server-info';

const { Header, Content, Footer } = Layout;
const { Title } = Typography;

const BaseLayout: SFC<BaseLayoutProps> = (props) => (
    <Layout className="base-layout__layout">
        <Header className="base-layout__header">
            <Row type="flex" justify="center" align="middle">
                <Col xs={6} lg={6}>
                    <Link to="/">
                        <Title level={1} className="base-layout__title">
                            <span className="base-layout__title-icon">🏅</span>
                            <span className="base-layout__title-text">Fuse8 CS:GO Statistics</span>
                        </Title>
                    </Link>
                </Col>
                <Col xs={12} lg={4}>
                    <ServerInfo />
                </Col>
                <Col xs={6} lg={6}>
                    <Navigation/>
                </Col>
            </Row>
        </Header>
        <Content className="base-layout__content">
            {props.children}
        </Content>

        <Footer className="base-layout__footer">
            <AuthorsCopyright />
            <div className="base-layout__footer-copyright">
                <Repository />
                <IconCopyright />
            </div>
        </Footer>
    </Layout>
);

type BaseLayoutProps = {
    children: ReactNode;
};

export default BaseLayout;
