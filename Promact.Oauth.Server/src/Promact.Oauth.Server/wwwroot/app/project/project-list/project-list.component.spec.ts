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

describe("Project List Test", () => {
    let projectListComponent: ProjectListComponent;
    class MockRouter { }
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provide(Router, { useClass: MockRouter }),
                provide(TestConnection, { useClass: TestConnection }),
                provide(ProjectService, { useClass: MockProjectService }),
                provide(Md2Toast, { useClass: MockToast }),
                provide(MockBaseService, { useClass: MockBaseService})
            ]
        });

    });

    beforeEach(inject([Router, ProjectService, Md2Toast], (router: Router, proService: ProjectService, toast: Md2Toast) => {
        projectListComponent = new ProjectListComponent(router, proService, toast);
    }));

     /**
     * get list of Projects
     */
    it("should get list of Project on initialization", () => {
        projectListComponent.ngOnInit();
        expect(projectListComponent.getPros());
    });

    
});
