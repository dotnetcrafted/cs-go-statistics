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
            item.tags.map((tags: any) => set.add(tags.Caption));
        });

        this.setState({
            tagsArr: Array.from(set)
        });
    }

    filter(tag: string): void {
        this.props.filteredByTag(tag, this.props.filtersTags);
    }

    refresh(): void {
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
                        <li onClick={() => this.filter(tag)} key={index} className='filter__item'>
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
    FilteredPosts: PostType[];
    filteredByTag: typeof filteredByTag;
    refreshPosts: typeof refreshPosts;
};

const mapStateToProps = (state: RootState) => {
    const FilteredPosts = state.app.FilteredPosts;
    return { FilteredPosts };
};

const mapDispatchToProps = {
    filteredByTag,
    refreshPosts
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);
