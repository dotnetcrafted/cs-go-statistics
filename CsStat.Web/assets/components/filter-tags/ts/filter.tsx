import React, { ReactNode } from 'react';

class Filter extends React.Component<FilterProps> {
    state = {
        tagsArr: []
    }

    componentDidMount(): void {
        const { posts } = this.props;
        const Arr: any[] = [];

        posts.forEach((post) => {
            post.tags.forEach((tag: any) => {
                if (!Arr.includes(tag.Caption)) {
                    Arr.push(tag.Caption);
                }
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
                    {this.state.tagsArr.map((tag, index) => (
                        <li key={index} onClick={() => this.props.postFilter(tag)} className="filter__item">{tag}</li>
                    ))}
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
