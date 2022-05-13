import React, { SFC } from 'react';
import { Provider } from 'react-redux';
import { Route, Switch } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import BaseLayout from '../../base-layout/ts/base-layout';
import configureStore, { history } from '../../../general/ts/redux/store';
import HomePage from '../../pages/ts/home-page';
import WikiPage from '../../pages/ts/wiki-page';
import NotFoundPage from '../../pages/ts/not-found-page';
import constants from '../../../general/ts/constants';
import { Matches } from 'components/matches';
import { Match } from 'components/match';
import { Weapons } from 'components/weapons';

const store = configureStore();

const App: SFC<AppProps> = (props) => (
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <BaseLayout>
                <Switch>
                    <Route exact path={constants.ROUTES.HOME} >
                        <HomePage playersDataUrl={props.playersDataUrl}/>
                    </Route>
                    <Route exact path={constants.ROUTES.WIKI} >
                        <WikiPage wikiDataApiPath={props.wikiDataApiPath}/>
                    </Route>
                    <Route exact path={constants.ROUTES.MATCHES} >
                        <Matches />
                    </Route>
                    <Route exact path={constants.ROUTES.MATCH_DETAILS} >
                        <Match />
                    </Route>
                    <Route exact path={constants.ROUTES.WEAPONS} >
                        <Weapons />
                    </Route>
                    <Route>
                        <NotFoundPage/>
                    </Route>
                </Switch>

            </BaseLayout>
        </ConnectedRouter>
    </Provider>
);

type AppProps = {
    playersDataUrl: string;
    wikiDataApiPath: string;
};

export default App;
