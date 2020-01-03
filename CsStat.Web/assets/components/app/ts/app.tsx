import React, { SFC } from 'react';
import { Provider } from 'react-redux';
import BaseLayout from '../../base-layout/ts/base-layout';
import configureStore from '../../../general/ts/redux/store';

const store = configureStore();

const App: SFC<AppProps> = (props) => (
    <Provider store={store}>
        <BaseLayout playersDataUrl={props.playersDataUrl}/>
    </Provider>
);

type AppProps = {
    playersDataUrl: string;
};

export default App;
