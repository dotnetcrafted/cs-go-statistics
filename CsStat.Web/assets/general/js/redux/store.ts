import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension/logOnlyInProduction';
import rootReducer from './reducers';

export default function configureStore() {
    const middleware = [thunk];

    const store = createStore(
        rootReducer,
        composeWithDevTools(
            applyMiddleware(...middleware)
        )
    );
  
    return store;
}

export type AppState = ReturnType<typeof rootReducer>;
