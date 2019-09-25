import React from 'react';
import ReactDOM from 'react-dom';
import { DcBaseComponent } from '@deleteagency/dc';
import HomePage from './home-page';
import PlayersData from '../../players-data/js/players-data';
import PlayerCard from '../../player-card/js/player-card';

export default class HomePageComponent extends DcBaseComponent {
    static getNamespace() {
        return 'home-page';
    }

    private getHomePageEl() {
        return React.createElement(
            HomePage,
            {
                playersData: this.getPlayersDataEl(),
                playerCard: this.getPlayersCardEl()
            },
            null
        );
    }

    private getPlayersDataEl() {
        return React.createElement(
            PlayersData,
            { playersDataUrl: this.options.playersDataUrl },
            null
        );
    }

    private getPlayersCardEl() {
        return React.createElement(PlayerCard, null, null);
    }

    onInit() {
        ReactDOM.render(this.getHomePageEl(), this.element);
    }
}
