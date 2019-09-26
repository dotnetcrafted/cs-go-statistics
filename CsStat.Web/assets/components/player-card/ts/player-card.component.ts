import React, { ReactElement } from 'react';
import ReactDOM from 'react-dom';
import { DcBaseComponent } from '@deleteagency/dc';
import PlayerCard from './player-card';

export default class PlayerCardComponent extends DcBaseComponent {
    static getNamespace(): string {
        return 'player-card';
    }

    private getPlayersCardEl(): ReactElement {
        return React.createElement(PlayerCard, null, null);
    }

    onInit(): void {
        ReactDOM.render(this.getPlayersCardEl(), this.element);
    }
}
