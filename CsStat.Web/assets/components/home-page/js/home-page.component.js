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

  onInit() {
    const {
      playersDataUrl
    } = this.options;

    ReactDOM.render(
            <HomePage
                playersData={<PlayersData playersDataUrl={playersDataUrl} />}
                playerCard={<PlayerCard />}
            >

            </HomePage>,
            this.element
    );
  }
}
