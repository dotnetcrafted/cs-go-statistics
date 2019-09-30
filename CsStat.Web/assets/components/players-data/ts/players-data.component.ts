import React, { ReactElement } from 'react';
import ReactDOM from 'react-dom';
import { DcBaseComponent } from '@deleteagency/dc';
import PlayersData from './players-data';

export default class PlayersDataComponent extends DcBaseComponent {
    static getNamespace(): string {
        return 'players-data';
    }

    private getPlayersDataEl(): ReactElement {
        const { playersDataUrl } = this.options;

        return React.createElement(PlayersData, { playersDataUrl }, null);
    }

    onInit(): void {
        ReactDOM.render(this.getPlayersDataEl(), this.element);
    }
}
