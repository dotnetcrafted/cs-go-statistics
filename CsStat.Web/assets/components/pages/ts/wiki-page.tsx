import React, { ReactNode } from 'react';
import '../scss/index.scss';
import { Row, Col, Empty } from 'antd';
import { connect } from 'react-redux';
import { fetchPosts, startRequest, stopRequest } from '../../../general/ts/redux/actions';
import { RootState, Post as PostType } from '../../../general/ts/redux/types';
import Post from '../../post';
import Filter from '../../filter';

class WikiPage extends React.Component<WikiPageProps> {
    fetchPosts(WikiDataApiPath: string): void {
        const url = new URL(WikiDataApiPath, window.location.origin);

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
        const { Posts, FilteredPosts } = this.props;

        if (Posts.length > 0) {
            console.log(this.props);
            return FilteredPosts.map((post: PostType, index: number) => <Post key={index} post={post} />);
        }

        return <Empty />;
    }

    componentDidMount(): void {
        this.fetchPosts(this.props.WikiDataApiPath);
    }

    render(): ReactNode {
        return (
            <React.Fragment>
                {this.props.Posts.length > 0 ? <Filter filtersTags={this.props.Posts} /> : null}

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
    Posts: PostType[];
    FilteredPosts: PostType[];
    IsLoading: boolean;
    WikiDataApiPath: string;
    fetchPosts: typeof fetchPosts;
};

const mapStateToProps = (state: RootState) => {
    const IsLoading = state.app.IsLoading;
    const Posts = state.app.Posts;
    const FilteredPosts = state.app.FilteredPosts;
    return { IsLoading, Posts, FilteredPosts };
};

export default connect(mapStateToProps, {
    fetchPosts,
    startRequest,
    stopRequest
})(WikiPage);
