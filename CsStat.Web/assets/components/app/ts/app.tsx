import React, { SFC } from 'react';
import { Provider } from 'react-redux';
import BaseLayout from '../../base-layout/ts/base-layout';
import configureStore from '../../../general/ts/redux/store';
import HomePage from '../../pages/ts/home-page';

const store = configureStore();

const App: SFC<AppProps> = (props) => (
    <Provider store={store}>
        <BaseLayout>
            <HomePage playersDataUrl={props.playersDataUrl}/>
        </BaseLayout>
    </Provider>
);

type AppProps = {
    playersDataUrl: string;
    wikiUrl: string;
};

export default App;
