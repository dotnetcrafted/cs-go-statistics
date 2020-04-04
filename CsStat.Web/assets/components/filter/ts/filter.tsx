import React, { ReactNode } from 'react';
import { connect } from 'react-redux';
import { filteredByTag, refreshPosts } from '../../../general/ts/redux/actions';
import { Post as PostType, RootState } from '../../../general/ts/redux/types';

class Filter extends React.Component<FilterProps> {
    state = {
        tagsArr: []
    };

    getListTags(): void {
        const set = new Set();

        this.props.filtersTags.forEach((item: any) => {
            item.tags.map((tags: any) => set.add(tags.caption));
        });

        this.setState({
            tagsArr: Array.from(set)
        });
    }

    filter(e: any, tag: string): void {
        e.target.classList.add('active');

        this.props.filteredByTag(tag, this.props.filtersTags);
    }

    refresh(): void {
        const div = document.querySelectorAll('.filter__item');
        div.forEach((item) => {
            if (item.classList.contains('active')) {
                item.classList.remove('active');
            }
        });
        this.props.refreshPosts(this.props.filtersTags);
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
    filtersTags: any;
    filteredPosts: PostType[];
    filteredByTag: typeof filteredByTag;
    refreshPosts: typeof refreshPosts;
};

const mapStateToProps = (state: RootState) => {
    const filteredPosts = state.app.filteredPosts;
    return { filteredPosts };
};

const mapDispatchToProps = {
    filteredByTag,
    refreshPosts
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);
