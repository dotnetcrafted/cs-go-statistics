import React, { ReactNode } from 'react';
import '../scss/index.scss';
import { Row, Col, Empty } from 'antd';
import { connect } from 'react-redux';
import { fetchPosts, startRequest, stopRequest } from '../../../general/ts/redux/actions';
import { RootState, Post as PostType } from '../../../general/ts/redux/types';
import Post from '../../post';
import Filter from '../../filter-tags';

class WikiPage extends React.Component<WikiPageProps> {
    state = {
        filtered: [],
        isFiltered: false
    }

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

    componentDidMount(): void {
        this.fetchPosts(this.props.WikiDataApiPath);
    }

    postFilter(tag: any) {
        if (tag !== 'all') {
            const filter: any[] = [];

            this.props.Posts.forEach((post) => {
                post.tags.forEach((item) => {
                    // eslint-disable-next-line no-unused-expressions
                    item.Caption === tag ? filter.push(post) : null;
                });
            });

            this.setState({
                filtered: filter,
                isFiltered: true
            });
        } else {
            this.setState({
                isFiltered: false
            });
        }
    }

    filter(): ReactNode {
        return this.state.filtered.map((post: PostType, index: number) => <Post key={index} post={post} />);
    }

    getPosts(): ReactNode {
        const { Posts } = this.props;

        if (Posts.length > 0) {
            return Posts.map((post: PostType, index: number) => <Post key={index} post={post} />);
        }
        return <Empty />;
    }

    render(): ReactNode {
        return (
            <div className="asdasdasdasdasdaadas">
                {this.props.Posts.length > 0 ?
                    <Filter posts={this.props.Posts} postFilter={(tag: any) => this.postFilter(tag)}/> :
                    <Empty />
                }

                <Row type="flex" justify="start" align="middle">
                    <Col xs={{ span: 24 }} lg={{ span: 12, offset: 6 }}>
                        {!this.state.isFiltered ? this.getPosts() : this.filter()}
                    </Col>
                </Row>
            </div>
        );
    }
}

type WikiPageProps = {
    Posts: PostType[];
    IsLoading: boolean;
    WikiDataApiPath: string;
    fetchPosts: typeof fetchPosts;
};

const mapStateToProps = (state: RootState) => {
    const IsLoading = state.app.IsLoading;
    const Posts = state.app.Posts;
    return { IsLoading, Posts };
};

export default connect(
    mapStateToProps,
    { fetchPosts, startRequest, stopRequest }
)(WikiPage);
