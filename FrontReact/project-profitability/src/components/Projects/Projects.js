import React from 'react';
import s from './Projects.module.css';
import Filter from "../Filter/Filter";
import ProjectBrief from "./ProjectBrief/ProjectBrief";

const Projects = () => {
    return <div className={s.body}>
        Body:Projects
        <Filter />
        <ProjectBrief />
    </div>
};

export default Projects;