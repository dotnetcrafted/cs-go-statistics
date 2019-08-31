import React from 'react';
import PropTypes from 'prop-types';

export default class LazyImage extends React.PureComponent {
    render() {
        const { image } = this.props;

        return image ? (
            <img className="lazyload" data-srcset={image.srcset} alt={image.alt} data-sizes="auto"/>
        ) : null;
    }
}
LazyImage.propTypes = {
    image: PropTypes.shape({
        srcset: PropTypes.string.isRequired,
        alt: PropTypes.string.isRequired
    })
};
