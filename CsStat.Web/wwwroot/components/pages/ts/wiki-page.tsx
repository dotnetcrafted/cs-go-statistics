import React, { ReactNode } from 'react';
import '../scss/index.scss';
import { Row, Col, Empty } from 'antd';
import { connect } from 'react-redux';
import { fetchPosts, startRequest, stopRequest } from '../../../general/ts/redux/actions';
import { RootState, Post as PostType } from '../../../general/ts/redux/types';
import Post from '../../post';

class WikiPage extends React.Component<WikiPageProps> {
    fetchPosts(wikiDataApiPath: string): void {
        const url = new URL(wikiDataApiPath, window.location.origin);

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: PostType[]) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;

                this.props.fetchPosts(data);
            })
            .catch((error) => {
                throw new Error(error);
            });
    }

    componentDidMount(): void {
        this.fetchPosts(this.props.wikiDataApiPath);
    }

    getPosts(): ReactNode {
        const { posts } = this.props;

        if (posts.length > 0) {
            return posts.map((post: PostType) => <Post key={post.id} post={post} />);
        }
        return <Empty />;
    }

    render(): ReactNode {
        return (
            <Row type="flex" justify="start" align="middle">
                <Col xs={{ span: 24 }} lg={{ span: 12, offset: 6 }}>
                    {this.getPosts()}
                </Col>
            </Row>
        );
    }
}

type WikiPageProps = {
    posts: PostType[];
    isLoading: boolean;
    wikiDataApiPath: string;
    fetchPosts: typeof fetchPosts;
};

const mapStateToProps = (state: RootState) => {
    return { 
        isLoading: state.app.isLoading,
        posts: state.app.posts,
    };
};

export default connect(
    mapStateToProps,
    { fetchPosts, startRequest, stopRequest }
)(WikiPage);
