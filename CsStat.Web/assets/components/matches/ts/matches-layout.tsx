import React from 'react';
import { Link } from 'react-router-dom';

const MatchesLayout = () => {
    return (
        <div>
            <h1>Matches</h1>
            <div>Filters</div>
            <Link to="/matches/test-id">test link to match details</Link>
            <div>
            </div>        
        </div>
    );
}

export default MatchesLayout;