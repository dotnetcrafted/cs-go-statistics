import React, { SFC } from 'react';
import '../scss/index.scss';

const WikiPage: SFC<WikiPageProps> = (props) => (
    <div className="wiki-page">
        <h1>Wiki Page</h1>
        <p>API URL: <a href={props.wikiUrl}>{props.wikiUrl}</a></p>
    </div>
);

type WikiPageProps = {
    wikiUrl: string;
};

export default WikiPage;
