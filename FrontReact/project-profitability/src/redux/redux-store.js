import {applyMiddleware, combineReducers, createStore} from "redux";
import thunkMiddleware from "redux-thunk";

let reducers = combineReducers({
    projectsPage: undefined,
    analyticsPage: undefined,
    auth: undefined,
    //form: formReducer
});


let store = createStore(reducers, applyMiddleware(thunkMiddleware));

//Для удобного дебага
window.store = store;
export default store;