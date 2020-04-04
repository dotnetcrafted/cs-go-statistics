import React, { ReactNode } from 'react';
import '../scss/index.scss';
// import { Row, Col, Empty } from 'antd';
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

    getPosts = (): ReactNode => {
        const { posts, filteredPost, tagsList } = this.props;

        console.log(this.props);

        if (posts.length > 0 && filteredPost.length === 0 && tagsList.length === 0) {
            return posts.map((post: any, index: number) => <Post key={index} post={post} />);
        }

        if (filteredPost.length > 0) {
            return filteredPost.map((post: any, index: number) => <Post key={index} post={post} />);
        }

        return null;
    };

    componentDidMount(): void {
        this.fetchPosts(this.props.wikiDataApiPath);
    }

    render(): ReactNode {
        const { posts } = this.props;
        return (
            <React.Fragment>
                <div className='posts'>
                    <div className='posts__inner'>
                        <div className='posts__content'>{this.getPosts()}</div>

                        <div className='posts__aside'>{posts.length > 0 ? <Filter /> : null}</div>
                    </div>
                </div>
            </React.Fragment>
        );
    }
}

type WikiPageProps = {
    posts: PostType[];
    filteredPost: PostType[];
    tagsList: string[];
    isLoading: boolean;
    wikiDataApiPath: string;
    fetchPosts: typeof fetchPosts;
};

const mapStateToProps = (state: RootState) => {
    return {
        isLoading: state.app.isLoading,
        posts: state.app.posts,
        filteredPost: state.app.filteredPost,
        tagsList: state.app.tagsList,
    };
};

export default connect(mapStateToProps, {
    fetchPosts,
    startRequest,
    stopRequest,
})(WikiPage);
