import { combineReducers } from 'redux';
import playersReducer from './playersReducer';


export default combineReducers({
    items: playersReducer
});
