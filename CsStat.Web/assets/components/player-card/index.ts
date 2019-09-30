import 'react-vis/dist/style.css';
import './scss/index.scss';

import { dcFactory } from '@deleteagency/dc';
import PlayersDataComponent from './ts/player-card.component';

dcFactory.register(PlayersDataComponent);
