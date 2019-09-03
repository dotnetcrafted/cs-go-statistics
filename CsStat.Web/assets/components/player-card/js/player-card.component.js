import React from 'react';
import ReactDOM from 'react-dom';
import DcBaseComponent from '../../../general/js/dc/dc-base-component';
import PlayerCard from './player-card';

export default class PlayerCardComponent extends DcBaseComponent {
    static getNamespace() {
        return 'player-card';
    }

    onInit() {
        ReactDOM.render(
            <PlayerCard />,
            this.element
        );
    }
}
