import React from 'react';
import s from './Navbar.module.css';
import {NavLink} from "react-router-dom";

const Navbar = () => {
    return <nav className={s.navbar}>
        <div className={s.item}>
            <NavLink to={"/projects"} activeClassName ={s.activeLink}>Проекты</NavLink>
        </div>
        <div className={`${s.item} ${s.activeLink}`}>
            <NavLink to={"/analitics"} activeClassName ={s.activeLink}>Аналитика</NavLink>
        </div>
    </nav>
};

export default Navbar;