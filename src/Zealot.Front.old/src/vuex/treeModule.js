import { logAction, logMutation } from '../helpers/consoleHelpers';
import { buildTreeMapAndSetParentsIds, moveNode } from '../helpers/treeHelper';
import { setPackContext } from '../services/packContextService';

const state = {
  selectedNodeId: 0,
  selectedNodeType: '',
  tree: {},
  nodesMap: null,
  draggedNode: null,
  isDragging: false
};

const getters = {
  selectedNodeId: currentState => currentState.selectedNodeId,
  selectedNodeType: currentState => currentState.selectedNodeType,
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
  selectNode(context, nodeId) {
    logAction('selectNode', nodeId);
    const node = context.state.nodesMap.get(nodeId);
    context.commit('setSelectedNode', node);
    if (node.type === 'request') {
      context.dispatch('requestModule/resetFromTree', {
        requestUrl: node.requestUrl,
        httpMethod: node.httpMethod
      }, { root: true });
    }
  },
  setNodeProperties(context, nodeProperties) {
    const isPristine = false;
    const nodeId = context.state.selectedNodeId;

    logAction('setNodeProperties', nodeId, isPristine);
    context.commit('setNodeProperties', { nodeId, isPristine, ...nodeProperties });
  }
};

const mutations = {
  setSelectedNode(currentState, node) {
    // currentState.selectedNode = { ...node };
    currentState.selectedNodeId = node.id;
    currentState.selectedNodeType = node.type;

  },
  initializeTree(currentState, tree) {
    let nodesMap = new Map();
    nodesMap = buildTreeMapAndSetParentsIds(tree, null, nodesMap);
    currentState.tree = tree;
    currentState.nodesMap = nodesMap;
    // currentState.selectedNode = { ...tree };
    currentState.selectedNodeId = tree.id;
    currentState.selectedNodeType = tree.type;
    setPackContext('a', { stuff: 0 });
  },
  setNodesMap(currentState, nodesMap) {
    currentState.nodesMap = nodesMap;
  },
  setDraggedNode(currentState, draggedNode) {
    currentState.draggedNode = draggedNode;
  },
  setIsDragging(currentState, value) {
    currentState.isDragging = value;
  },
  setNodeInTree(currentState, nodeProperties) {
    // retrieve node from the main Map
    let node = currentState.nodesMap.get(nodeProperties.id); // eslint-disable-line
    // set new properties in place
    Object.assign(node, nodeProperties);
  },
  setNodeProperties(currentState, nodeProperties) {
    logMutation('[setNodeProperties]', nodeProperties);

    // retrieve node from the main Map
    let node = currentState.nodesMap.get(currentState.selectedNodeId); // eslint-disable-line
    // set new properties in place
    Object.assign(node, nodeProperties);
    // replace the selectedNode so that Vue recycles computed properties
    currentState.selectedNodeId = node.id;
    currentState.selectedNodeType = node.type;
  }
};

export default {
  strict: true,
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
