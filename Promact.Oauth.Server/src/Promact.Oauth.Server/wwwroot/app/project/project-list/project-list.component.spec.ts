import {async,inject,TestBed,ComponentFixture} from '@angular/core/testing';
import {provide} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model';
import { Component } from '@angular/core';
import { DeprecatedFormsModule } from '@angular/common';
import {ROUTER_DIRECTIVES, Router } from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {ProjectListComponent} from '../project-list/Project-list.Component';
import {MockToast} from "../../shared/mocks/mock.toast";
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
import {MockBaseService} from '../../shared/mocks/mock.base';
import { LoginService } from '../../login.service';
declare var describe, it, beforeEach, expect;

describe("Project List Test", () => {
    let projectListComponent: ProjectListComponent;
    class MockRouter { }
    class McokLogin { }
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provide(Router, { useClass: MockRouter }),
                provide(TestConnection, { useClass: TestConnection }),
                provide(ProjectService, { useClass: MockProjectService }),
                provide(Md2Toast, { useClass: MockToast }),
                provide(MockBaseService, { useClass: MockBaseService }),
                provide(LoginService, { useClass: McokLogin }),
            ]
        });

    });

    beforeEach(inject([Router, ProjectService, Md2Toast], (router: Router, proService: ProjectService, toast: Md2Toast, loginService: LoginService) => {
        projectListComponent = new ProjectListComponent(router, proService, toast, loginService);
    }));

    it("should be defined", () => {
        expect(projectListComponent).toBeDefined();
    });
     
    it("should get list of Project on initialization", () => {
        projectListComponent.getProjects();
        expect(projectListComponent.projects.length).toEqual(1);
    });

    
});
