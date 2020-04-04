import React, { ReactNode } from 'react';
import '../scss/index.scss';
import { Row, Col, Empty } from 'antd';
import { connect } from 'react-redux';
import { fetchPosts, startRequest, stopRequest } from '../../../general/ts/redux/actions';
import { RootState, Post as PostType } from '../../../general/ts/redux/types';
import Post from '../../post';
import Filter from '../../filter';

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

    getPosts(): ReactNode {
        const { posts, filteredPosts } = this.props;

        if (posts.length > 0) {
            console.log(this.props);
            return filteredPosts.map((post: PostType, index: number) => <Post key={index} post={post} />);
        }

        return <Empty />;
    }

    componentDidMount(): void {
        this.fetchPosts(this.props.wikiDataApiPath);
    }

    render(): ReactNode {
        return (
            <React.Fragment>
                {this.props.posts.length > 0 ? <Filter filtersTags={this.props.posts} /> : null}

                <Row type='flex' justify='start' align='middle'>
                    <Col xs={{ span: 24 }} lg={{ span: 12, offset: 6 }}>
                        {this.getPosts()}
                    </Col>
                </Row>
            </React.Fragment>
        );
    }
}

type WikiPageProps = {
    posts: PostType[];
    filteredPosts: PostType[];
    isLoading: boolean;
    wikiDataApiPath: string;
    fetchPosts: typeof fetchPosts;
};

const mapStateToProps = (state: RootState) => {
    return { 
        isLoading: state.app.isLoading,
        posts: state.app.posts,
        filteredPosts: state.app.filteredPosts
    };
};

export default connect(mapStateToProps, {
    fetchPosts,
    startRequest,
    stopRequest
})(WikiPage);
