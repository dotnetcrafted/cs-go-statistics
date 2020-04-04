import React, { ReactNode } from 'react';
import { connect } from 'react-redux';
import { filteredByTag, refreshPosts } from '../../../general/ts/redux/actions';
import { Post as PostType, RootState } from '../../../general/ts/redux/types';

class Filter extends React.Component<FilterProps> {
    state = {
        tagsArr: [],
    };

    getListTags(): void {
        const set = new Set();

        this.props.posts.forEach((item: any) => {
            item.tags.map((tags: any) => set.add(tags.caption));
        });

        this.setState({
            tagsArr: Array.from(set),
        });
    }

    filter(e: any, tag: string): void {
        const { tagsList, posts } = this.props;

        if (tagsList.includes(tag)) {
            tagsList.splice(tagsList.indexOf(tag), 1);
            e.target.classList.remove('active');
        } else {
            tagsList.push(tag);
            e.target.classList.add('active');
        }

        this.props.filteredByTag(tag, tagsList, posts);
    }

    refresh(): void {
        const div = document.querySelectorAll('.filter__item');
        div.forEach((item) => {
            if (item.classList.contains('active')) {
                item.classList.remove('active');
            }
        });
        this.props.refreshPosts(this.props.posts);
    }

    componentDidMount(): void {
        this.getListTags();
    }

    render(): ReactNode {
        return (
            <div className='filter'>
                <ul className='filter__list'>
                    <li onClick={() => this.refresh()} className='filter__item'>
                        all
                    </li>
                    {this.state.tagsArr.map((tag, index) => (
                        <li onClick={(e) => this.filter(e, tag)} key={index} className='filter__item'>
                            {tag}
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

type FilterProps = {
    posts: PostType[];
    tagsList: string[];
    filteredByTag: typeof filteredByTag;
    refreshPosts: typeof refreshPosts;
};

const mapStateToProps = (state: RootState) => {
    return {
        posts: state.app.posts,
        tagsList: state.app.tagsList,
    };
};

const mapDispatchToProps = {
    filteredByTag,
    refreshPosts,
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);
