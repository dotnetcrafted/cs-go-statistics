import React, { ReactNode } from 'react';
import { Empty } from 'antd';
import '../scss/index.scss';
import { connect } from 'react-redux';
import { fetchPosts, startRequest, stopRequest } from '../../../general/ts/redux/actions';
import { RootState, Post } from '../../../general/ts/redux/types';

class WikiPage extends React.Component<WikiPageProps> {
    fetchPosts(WikiDataApiPath: string): void {
        const url = new URL(WikiDataApiPath);

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: Post[]) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;

                this.props.fetchPosts(data);
            })
            .catch((error) => {
                throw new Error(error);
            });
    }

    render(): ReactNode {
        this.fetchPosts(this.props.WikiDataApiPath);

        return (
            <Empty />
        );
    }
}

type WikiPageProps = {
    WikiDataApiPath: string;
    fetchPosts: typeof fetchPosts;
};

const mapStateToProps = (state: RootState) => {
    const IsLoading = state.app.IsLoading;
    return { IsLoading };
};

export default connect(
    mapStateToProps,
    { fetchPosts, startRequest, stopRequest }
)(WikiPage);
