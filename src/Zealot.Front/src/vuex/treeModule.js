import { logAction } from '../helpers/consoleHelpers';
import { buildTreeMapAndSetParentsIds, moveNode } from '../helpers/vue-drag-tree-utils';

const state = {
  selectedNode: { id: 0 },
  tree: {},
  nodesMap: null,
  draggedNode: null,
  isDragging: false
};

const getters = {
  selectedNode: currentState => currentState.selectedNode,
  tree: currentState => currentState.tree,
  nodesMap: currentState => currentState.nodesMap,
  draggedNode: currentState => currentState.draggedNode,
  defaultNewNodeName: () => 'New node',
  isDragging: currentState => currentState.isDragging
};

const actions = {
  setRawTree(context, tree) {
    context.commit('initializeTree', tree);
  },
  setDraggedNodeId(context, draggedNodeId) {
    logAction('setDraggedNodeId', draggedNodeId);
    const nodeInTree = context.state.nodesMap.get(draggedNodeId);
    context.commit('setDraggedNode', nodeInTree);
  },
  dropOn(context, dragTargetNodeId) {
    const nodeInTree = context.state.nodesMap.get(dragTargetNodeId);
    // context.commit('setDragTargetNode', nodeInTree);
    moveNode(context.state.draggedNode, nodeInTree, context.state.nodesMap);
  },
  setIsDragging(context, value) {
    context.commit('setIsDragging', value);
  },
  allowDrag(context, model) {
    logAction('allowDrag', model);
    // can be dragged
    return true;
  },
  selectNode(context, { id, name, type }) {
    // logAction('selectNode', model, component);
    context.commit('setSelectedNode', { id, name, type });
  }
};

const mutations = {
  setSelectedNode(currentState, { id, name, type }) {
    currentState.selectedNode = { id, name, type };
  },
  initializeTree(currentState, tree) {
    let nodesMap = new Map();
    nodesMap = buildTreeMapAndSetParentsIds(tree, null, nodesMap);
    currentState.tree = tree;
    currentState.nodesMap = nodesMap;
    currentState.selectedNode = { id: tree.id, name: tree.name, type: tree.type };
  },
  setNodesMap(currentState, nodesMap) {
    currentState.nodesMap = nodesMap;
  },
  setDraggedNode(currentState, draggedNode) {
    currentState.draggedNode = draggedNode;
  },
  setIsDragging(currentState, value) {
    currentState.isDragging = value;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
