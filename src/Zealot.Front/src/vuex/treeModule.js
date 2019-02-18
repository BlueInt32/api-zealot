import { logAction } from '../helpers/consoleHelpers';
import { buildTreeMapAndSetParentsIds, moveNode } from '../helpers/vue-drag-tree-utils';

const state = {
  currentlySelectedId: null,
  draggedNode: null,
  draggingTarget: null,
  tree: {},
  nodesMap: null
};

const getters = {
  tree: currentState => currentState.tree,
  nodesMap: currentState => currentState.nodesMap,
  draggedNode: currentState => currentState.draggedNode,
  defaultNewNodeName: () => 'New node',
  currentlySelectedId: currentState => currentState.currentlySelectedId
};

const actions = {
  setRawTree(context, tree) {
    context.commit('initializeTree', tree);
  },
  setDraggedNodeId(context, draggedNodeId) {
    const nodeInTree = context.state.nodesMap.get(draggedNodeId);
    context.commit('setDraggedNode', nodeInTree);
  },
  dropOn(context, dragTargetNodeId) {
    const nodeInTree = context.state.nodesMap.get(dragTargetNodeId);
    // context.commit('setDragTargetNode', nodeInTree);
    moveNode(context.state.draggedNode, nodeInTree, context.state.nodesMap);
  },
  allowDrag(context, model) {
    logAction('allowDrag', model);
    // can be dragged
    return true;
  },
  curNodeClicked(context, { node }) {
    // logAction('curNodeClicked', model, component);
    context.commit('setSelectedId', { id: node.id });
  },
  dragHandler() {
    // console.log('dragHandler: ', model, component, e);
  },
  dragEnterHandler() {
    // console.log('dragEnterHandler: ', model, component, e);
  },
  dragLeaveHandler() {
    // console.log('dragLeaveHandler: ', model, component, e);
  },
  dragOverHandler() {
    // console.log('dragOverHandler: ', model, component, e);
  },
  dragEndHandler() {
    // console.log('dragEndHandler: ', model, component, e);
  },
  dropHandler() {
    // console.log('dropHandler: ', model, component, e);
  }
};

const mutations = {
  setSelectedId(currentState, { id }) {
    currentState.currentlySelectedId = id;
  },
  initializeTree(currentState, tree) {
    let nodesMap = new Map();
    nodesMap = buildTreeMapAndSetParentsIds(tree, null, nodesMap);
    currentState.tree = tree;
    currentState.nodesMap = nodesMap;
  },
  setNodesMap(currentState, nodesMap) {
    currentState.nodesMap = nodesMap;
  },
  setDraggedNode(currentState, draggedNode) {
    currentState.draggedNode = draggedNode;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
