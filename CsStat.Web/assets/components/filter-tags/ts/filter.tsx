import React, { ReactNode } from 'react';

class Filter extends React.Component<FilterProps> {
    state = {
        tagsArr: []
    }

    componentDidMount(): void {
        const { posts } = this.props;
        const Arr: any[] = [];
        let id = 0;

        posts.forEach((post) => {
            post.tags.forEach((tag: any) => {
                Arr.push(
                    <li key={id} onClick={() => this.props.postFilter(tag.Caption)} className="filter__item">{tag.Caption}</li>
                );
                id++;
            });
        });

        this.setState({
            tagsArr: Arr
        });
    }

    render(): ReactNode {
        return (
            <div className="filter">
                <ul className="filter__list">
                    <li onClick={() => this.props.postFilter('all')} className="filter__item">all</li>
                    {this.state.tagsArr}
                </ul>
            </div>
        );
    }
}

type FilterProps = {
    posts: any[];
    postFilter: any
};

export default Filter;
