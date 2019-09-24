import React from 'react';
import ReactDOM from 'react-dom';
import { DcBaseComponent } from '@deleteagency/dc';
import PlayersData from './players-data';

export default class PlayersDataComponent extends DcBaseComponent {
  static getNamespace() {
    return 'players-data';
  }

  onInit() {
    const {
      playersDataUrl
    } = this.options;

    ReactDOM.render(
            <PlayersData
                playersDataUrl={playersDataUrl}
            />,
            this.element
    );
  }
}
