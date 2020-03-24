import store from '.';
import {
  VuexModule,
  Module,
  Action,
  getModule,
  Mutation
} from 'vuex-module-decorators';
import AppService from '@/app.service';
import { logAction, logMutation, log } from '../helpers/consoleHelpers';
import Project from '@/domain/Project';
import Guid from '@/helpers/Guid';
import { Node, RequestNode } from '@/domain/Node';
import { NodeType } from '@/domain/NodeType';
import * as treeHelper from '@/helpers/treeHelper';
import * as packContextService from '@/domain/packContextService';
import RequestModule from './RequestModule';

const appService = new AppService();

@Module({
  dynamic: true,
  namespaced: true,
  name: 'TreeModule',
  store
})
export default class TreeModule extends VuexModule {
  public nodesMap: Map<string, Node> = new Map<string, Node>();
  public draggedNode: Node | null = null;
  public selectedNodeId: string = '';
  public selectedNodeType: NodeType | null = null;
  public tree: Node = new Node();
  public isDragging: boolean = false;

  @Action({ rawError: true })
  public setRawTree(tree: Node) {
    // TODO maybe could be a Mutation
    this.context.commit('initializeTree', tree);
  }

  @Action({ rawError: true })
  public setDraggedNodeId(draggedNodeId: string) {
    // TODO maybe could be a Mutation
    logAction('setDraggedNodeId', draggedNodeId);
    const nodeInTree = this.nodesMap.get(draggedNodeId);
    this.context.commit('setDraggedNode', nodeInTree);
  }

  @Action({ rawError: true })
  public dropOn(dragTargetNodeId: string) {
    // TODO maybe could be a Mutation
    const nodeInTree = this.nodesMap.get(dragTargetNodeId);
    // context.commit('setDragTargetNode', nodeInTree);
    if (this.draggedNode && nodeInTree) {
      treeHelper.moveNode(this.draggedNode, nodeInTree, this.nodesMap);
    }
  }

  // @Action({ rawError: true })
  // allowDrag(model) {
  //   // TODO maybe could be a Mutation
  //   logAction('allowDrag', model);
  //   // can be dragged
  //   return true;
  // }

  @Action({ rawError: true })
  public selectNode(nodeId: string) {
    // TODO maybe could be a Mutation
    logAction('selectNode', nodeId);
    const node = this.nodesMap.get(nodeId);
    if (node) {
      this.context.commit('setSelectedNode', node);
      if (node.type === 'request') {
        const requestModule = getModule(RequestModule);
        const requestNode = node as RequestNode;
        console.log(requestNode);
        requestModule.setcurrentNode(requestNode);
      }
    }
  }

  @Mutation
  public setSelectedNode(node: Node) {
    // currentState.selectedNode = { ...node };
    this.selectedNodeId = node.id;
    this.selectedNodeType = node.type;
  }

  @Mutation
  public initializeTree(tree: Node) {
    let nodesMap = new Map();
    nodesMap = treeHelper.buildTreeMapAndSetParentsIds(tree, null, nodesMap);
    this.tree = tree;
    this.nodesMap = nodesMap;
    // currentState.selectedNode = { ...tree };
    this.selectedNodeId = tree.id;
    this.selectedNodeType = tree.type;
    packContextService.setPackContext('a', { stuff: 0 });
  }

  @Mutation
  public setNodesMap(nodesMap: Map<string, Node>) {
    this.nodesMap = nodesMap;
  }

  @Mutation
  public setDraggedNode(draggedNode: Node) {
    this.draggedNode = draggedNode;
  }

  @Mutation
  public setIsDragging(value: boolean) {
    this.isDragging = value;
  }

  @Mutation
  public setNodeInTree(node: Node) {
    // retrieve node from the main Map
    let nodeInTree = this.nodesMap.get(node.id);
    // set new properties in place
    Object.assign(nodeInTree, node);
  }
}
