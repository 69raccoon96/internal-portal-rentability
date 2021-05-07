//import logo from './logo.svg';
import './App.css';
import Navbar from "./components/Navbar/Navbar";
import Projects from "./components/Projects/Projects";
import Header from "./components/Header/Header";
import React from "react";
import {Route} from "react-router-dom";

function App() {
    return (
        <div className={"app-wrapper"}>
            <Header/>
            <Navbar/>
            <div className={"app-wrapper-content"}>
                <Route exact path={"/projects"} render={() => <Projects/>}/>
                <Route path={"/projects"} render={() => undefined}/>
            </div>

        </div>
    );
}

export default App;
