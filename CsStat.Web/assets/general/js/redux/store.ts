import { createStore } from 'redux';
import rootReducer from './reducers';
import { IAppState, ActionTypes } from './types';


export default function configureStore() {    
    const store = createStore<IAppState, ActionTypes, IAppState, any>(
        rootReducer,
        {},
    );
  
    return store;
}

export type AppState = ReturnType<typeof rootReducer>;
