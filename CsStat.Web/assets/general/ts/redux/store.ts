import { createStore } from 'redux';
import rootReducer from './reducers';
import { AppState, ActionTypes } from './types';


export default function configureStore() {    
    const store = createStore<AppState, ActionTypes, AppState, any>(
        rootReducer,
        {},
    );
  
    return store;
}

export type AppState = ReturnType<typeof rootReducer>;
