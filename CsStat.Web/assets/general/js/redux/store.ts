import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension/logOnlyInProduction';
import rootReducer from './reducers';
import { IAppState } from './types';

const initialState: IAppState = {
    IsLoading: false,
    Players: [],
    SelectedPlayer: '',
    DateFrom: '',
    DateTo: ''
}
export default function configureStore() {
    const middleware = [thunk];

    const store = createStore<IAppState>(
        rootReducer,
        initialState,
        composeWithDevTools(
            applyMiddleware(...middleware)
        )
    );
  
    return store;
}

export type AppState = ReturnType<typeof rootReducer>;
