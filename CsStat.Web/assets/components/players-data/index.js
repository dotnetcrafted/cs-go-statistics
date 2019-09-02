import './scss/index.scss';
import 'antd/dist/antd.css';

import dcFactory from '../../general/js/dc/dc-factory';
import PlayersDataComponent from './js/players-data.component';

dcFactory.register(PlayersDataComponent);
