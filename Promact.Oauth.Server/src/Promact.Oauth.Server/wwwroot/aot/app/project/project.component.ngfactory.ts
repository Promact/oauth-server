/**
 * This file is generated by the Angular 2 template compiler.
 * Do not edit.
 */
 /* tslint:disable */

import * as import0 from '../../../app/project/project.component';
import * as import1 from '@angular/core/src/linker/view';
import * as import2 from '@angular/core/src/render/api';
import * as import3 from '@angular/core/src/linker/element';
import * as import4 from '../../../app/project/project.service';
import * as import5 from '@angular/core/src/linker/view_utils';
import * as import6 from '@angular/core/src/di/injector';
import * as import7 from '@angular/core/src/linker/view_type';
import * as import8 from '@angular/core/src/change_detection/change_detection';
import * as import9 from '../../../app/http.service';
import * as import10 from '../../../app/login.service';
import * as import11 from '@angular/router/src/router';
import * as import12 from '../../../app/shared/userrole.model';
import * as import13 from '@angular/core/src/metadata/view';
import * as import14 from '@angular/core/src/linker/component_factory';
import * as import15 from '../../node_modules/@angular/router/src/directives/router_outlet.ngfactory';
import * as import16 from '@angular/router/src/router_outlet_map';
import * as import17 from '@angular/core/src/linker/component_factory_resolver';
import * as import18 from '@angular/router/src/directives/router_outlet';
export class Wrapper_ProjectComponent {
  context:import0.ProjectComponent;
  changed:boolean;
  constructor(p0:any,p1:any,p2:any) {
    this.changed = false;
    this.context = new import0.ProjectComponent(p0,p1,p2);
  }
  detectChangesInternal(view:import1.AppView<any>,el:any,throwOnChange:boolean):boolean {
    var changed:any = this.changed;
    this.changed = false;
    if (!throwOnChange) { if ((view.numberOfChecks === 0)) { this.context.ngOnInit(); } }
    return changed;
  }
}
var renderType_ProjectComponent_Host:import2.RenderComponentType = (null as any);
class _View_ProjectComponent_Host0 extends import1.AppView<any> {
  _el_0:any;
  /*private*/ _appEl_0:import3.AppElement;
  _ProjectComponent_0_4:Wrapper_ProjectComponent;
  __ProjectService_0_5:import4.ProjectService;
  constructor(viewUtils:import5.ViewUtils,parentInjector:import6.Injector,declarationEl:import3.AppElement) {
    super(_View_ProjectComponent_Host0,renderType_ProjectComponent_Host,import7.ViewType.HOST,viewUtils,parentInjector,declarationEl,import8.ChangeDetectorStatus.CheckAlways);
  }
  get _ProjectService_0_5():import4.ProjectService {
    if ((this.__ProjectService_0_5 == (null as any))) { (this.__ProjectService_0_5 = new import4.ProjectService(this.parentInjector.get(import9.HttpService))); }
    return this.__ProjectService_0_5;
  }
  createInternal(rootSelector:string):import3.AppElement {
    this._el_0 = this.selectOrCreateHostElement('ng-component',rootSelector,(null as any));
    this._appEl_0 = new import3.AppElement(0,(null as any),this,this._el_0);
    var compView_0:any = viewFactory_ProjectComponent0(this.viewUtils,this.injector(0),this._appEl_0);
    this._ProjectComponent_0_4 = new Wrapper_ProjectComponent(this.parentInjector.get(import10.LoginService),this.parentInjector.get(import11.Router),this.parentInjector.get(import12.UserRole));
    this._appEl_0.initComponent(this._ProjectComponent_0_4.context,([] as any[]),compView_0);
    compView_0.create(this._ProjectComponent_0_4.context,this.projectableNodes,(null as any));
    this.init(([] as any[]).concat([this._el_0]),[this._el_0],([] as any[]),([] as any[]));
    return this._appEl_0;
  }
  injectorGetInternal(token:any,requestNodeIndex:number,notFoundResult:any):any {
    if (((token === import0.ProjectComponent) && (0 === requestNodeIndex))) { return this._ProjectComponent_0_4.context; }
    if (((token === import4.ProjectService) && (0 === requestNodeIndex))) { return this._ProjectService_0_5; }
    return notFoundResult;
  }
  detectChangesInternal(throwOnChange:boolean):void {
    this._ProjectComponent_0_4.detectChangesInternal(this,this._el_0,throwOnChange);
    this.detectContentChildrenChanges(throwOnChange);
    this.detectViewChildrenChanges(throwOnChange);
  }
}
function viewFactory_ProjectComponent_Host0(viewUtils:import5.ViewUtils,parentInjector:import6.Injector,declarationEl:import3.AppElement):import1.AppView<any> {
  if ((renderType_ProjectComponent_Host === (null as any))) { (renderType_ProjectComponent_Host = viewUtils.createRenderComponentType('',0,import13.ViewEncapsulation.None,([] as any[]),{})); }
  return new _View_ProjectComponent_Host0(viewUtils,parentInjector,declarationEl);
}
export const ProjectComponentNgFactory:import14.ComponentFactory<import0.ProjectComponent> = new import14.ComponentFactory<import0.ProjectComponent>('ng-component',viewFactory_ProjectComponent_Host0,import0.ProjectComponent);
const styles_ProjectComponent:any[] = ([] as any[]);
var renderType_ProjectComponent:import2.RenderComponentType = (null as any);
class _View_ProjectComponent0 extends import1.AppView<import0.ProjectComponent> {
  _text_0:any;
  _el_1:any;
  /*private*/ _appEl_1:import3.AppElement;
  _RouterOutlet_1_5:import15.Wrapper_RouterOutlet;
  _text_2:any;
  constructor(viewUtils:import5.ViewUtils,parentInjector:import6.Injector,declarationEl:import3.AppElement) {
    super(_View_ProjectComponent0,renderType_ProjectComponent,import7.ViewType.COMPONENT,viewUtils,parentInjector,declarationEl,import8.ChangeDetectorStatus.CheckAlways);
  }
  createInternal(rootSelector:string):import3.AppElement {
    const parentRenderNode:any = this.renderer.createViewRoot(this.declarationAppElement.nativeElement);
    this._text_0 = this.renderer.createText(parentRenderNode,'\n    ',(null as any));
    this._el_1 = this.renderer.createElement(parentRenderNode,'router-outlet',(null as any));
    this._appEl_1 = new import3.AppElement(1,(null as any),this,this._el_1);
    this._RouterOutlet_1_5 = new import15.Wrapper_RouterOutlet(this.parentInjector.get(import16.RouterOutletMap),this._appEl_1.vcRef,this.parentInjector.get(import17.ComponentFactoryResolver),(null as any));
    this._text_2 = this.renderer.createText(parentRenderNode,'\n    ',(null as any));
    this.init(([] as any[]),[
      this._text_0,
      this._el_1,
      this._text_2
    ]
    ,([] as any[]),([] as any[]));
    return (null as any);
  }
  injectorGetInternal(token:any,requestNodeIndex:number,notFoundResult:any):any {
    if (((token === import18.RouterOutlet) && (1 === requestNodeIndex))) { return this._RouterOutlet_1_5.context; }
    return notFoundResult;
  }
  detectChangesInternal(throwOnChange:boolean):void {
    this._RouterOutlet_1_5.detectChangesInternal(this,this._el_1,throwOnChange);
    this.detectContentChildrenChanges(throwOnChange);
    this.detectViewChildrenChanges(throwOnChange);
  }
  destroyInternal():void {
    this._RouterOutlet_1_5.context.ngOnDestroy();
  }
}
export function viewFactory_ProjectComponent0(viewUtils:import5.ViewUtils,parentInjector:import6.Injector,declarationEl:import3.AppElement):import1.AppView<import0.ProjectComponent> {
  if ((renderType_ProjectComponent === (null as any))) { (renderType_ProjectComponent = viewUtils.createRenderComponentType('',0,import13.ViewEncapsulation.None,styles_ProjectComponent,{})); }
  return new _View_ProjectComponent0(viewUtils,parentInjector,declarationEl);
}