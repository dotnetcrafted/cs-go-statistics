import '../scss/index.scss';
import React, { SFC, ReactNode } from 'react';
import { Link } from 'react-router-dom';
import { Layout, Row, Col } from 'antd';
import { getIconByName } from 'project/helpers';
import IconCopyright from 'components/icon-copyright';
import AuthorsCopyright from 'components/authors-copyright';
import Repository from 'components/repository';
import Navigation from 'components/navigation';
import { ServerInfo } from 'components/server-info';

const { Header, Content, Footer } = Layout;

type BaseLayoutProps = {
    children: ReactNode;
};

const BaseLayout: SFC<BaseLayoutProps> = (props) => {
    const logoIcon = getIconByName('logo');

    return (
        <Layout className="base-layout__layout">
            <Header className="base-layout__header">
                <Row type="flex" justify="center" align="middle">
                    <Col xs={6} lg={6}>
                        <Link className="base-layout__logo" to="/">
                            {
                                logoIcon &&
                                <img src={logoIcon.image} alt="Fuse8 CS:GO Statistics" />
                            }
                        </Link>
                    </Col>
                    <Col xs={12} lg={4}>
                        <ServerInfo />
                    </Col>
                    <Col xs={6} lg={6}>
                        <Navigation />
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
};

export default BaseLayout;
