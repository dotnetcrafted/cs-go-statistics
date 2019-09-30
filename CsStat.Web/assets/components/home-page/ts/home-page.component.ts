import React, { ReactElement } from 'react';
import ReactDOM from 'react-dom';
import { DcBaseComponent } from '@deleteagency/dc';
import HomePage from './home-page';
import PlayersData from '../../players-data/ts/players-data';
import PlayerCard from '../../player-card/ts/player-card';

export default class HomePageComponent extends DcBaseComponent {
    static getNamespace(): string {
        return 'home-page';
    }

    private getHomePageEl(): ReactElement {
        return React.createElement(
            HomePage,
            {
                playersData: this.getPlayersDataEl(),
                playerCard: this.getPlayersCardEl()
            },
            null
        );
    }

    private getPlayersDataEl(): ReactElement {
        return React.createElement(PlayersData, { playersDataUrl: this.options.playersDataUrl }, null);
    }

    private getPlayersCardEl(): ReactElement {
        return React.createElement(PlayerCard, null, null);
    }

    onInit(): void {
        ReactDOM.render(this.getHomePageEl(), this.element);
    }
}
