import React from 'react';
import ReactDOM from 'react-dom';
import DcBaseComponent from '../../../general/js/dc/dc-base-component';
import HomePage from './home-page';
import PlayersData from '../../players-data/js/players-data';

export default class HomePageComponent extends DcBaseComponent {
    static getNamespace() {
        return 'home-page';
    }

    onInit() {
        const {
            playersDataUrl
        } = this.options;

        ReactDOM.render(
            <HomePage>
                <PlayersData
                    playersDataUrl={playersDataUrl}
                />
            </HomePage>,
            this.element
        );
    }
}
