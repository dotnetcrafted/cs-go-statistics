import React, { SFC } from 'react';
import { Empty } from 'antd';
import '../scss/index.scss';

const WikiPage: SFC<WikiPageProps> = (props) => (
    <Empty />
);

type WikiPageProps = {
    wikiUrl: string;
};

export default WikiPage;
