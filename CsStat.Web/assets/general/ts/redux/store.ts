import { createStore } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension/logOnlyInProduction';
import rootReducer from './reducers';
import { AppState, ActionTypes } from './types';

export default function configureStore() {
    const store = createStore<AppState, ActionTypes, AppState, any>(
        rootReducer,
        {},
        composeWithDevTools()
    );

    return store;
}

export type AppState = ReturnType<typeof rootReducer>;
