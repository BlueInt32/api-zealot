/* eslint-disable */

const node2IsDirectParentOfNode1 = (node1, node2) => node1.parentId === node2.id;

const buildTreeMapAndSetParentsIds = (node, parent, nodesMap) => {
  node.parentId = parent ? parent.id : null;
  node.isPristine = true;
  nodesMap.set(node.id, node);
  if(node.children) {
    node.children.forEach(child => {
      buildTreeMapAndSetParentsIds(child, node, nodesMap);
    });
  }
  return nodesMap;
};


const prepareTreeBeforeSave = (node) => {
  if(node.type === 'request') {
    prepareRequestNode(node);
  }
  if(node.children) {
    node.children.forEach(child => {
      prepareTreeBeforeSave(child);
    });
  }
};

const prepareRequestNode = (node) => {
  node.attributes = {
    httpMethod: node.httpMethod,
    requestUrl: node.requestUrl
  };
}

const node2IsAncestorOfNode1 = function (node1, node2, nodesMap) {
  if (node1.parentId === null) {
    // if node1 is the root node, return false right away
    return false;
  }

  let currentNode = node1;

  while(currentNode.parentId) {
    if(currentNode.parentId === node2.id) {
      return true;
    }
    currentNode = nodesMap.get(currentNode.parentId);
  }

  return false;
};

const moveNode = (node1, node2, nodesMap) => {

  // having "P(S)" = direct Parent of S, and "X ∈ Y" = X is a descendant of Y
  // prevent-drop rules : for "Source S droping on Target T", the following structures
  // will prevent the drop to occur
  // 1/ T isn't a pack
  // 2/ S == T, cannot drop a node in itself
  // 3/ P(S) == T, cannot drop a node where it sits already
  // 4/ T ∈ S, cannot drop a node in one of its children

  // rule 1: if T isn't a pack, cannot drop inside it
  if(node2.type !== 'pack') {
    return;
  }

  // rule 2: if S == T, block because one cannot drop a node in itself
  if (node1.id === node2.id) {
    return;
  }

  // rule 3: if P(S) == T, block because one cannot drop a node where it sits already
  if (node2IsDirectParentOfNode1(node1, node2)) {
    return;
  }

  // rule 4: if 'T ∈ S' (or 'S is ancestor of T'), block because one cannot drop a node into one of its descendant
  if (node2IsAncestorOfNode1(node2, node1, nodesMap)) {
    return;
  }

  // remove the moved node from its previous parent
  let node1Parent = nodesMap.get(node1.parentId);
  node1Parent.children = node1Parent.children.filter(
    item => item.id !== node1.id
  );
  // set node1's parent to node2
  node1.parentId = node2.id;

  // add the moved node to the target node's children
  node2.children = [...node2.children, node1];
};

export {
 node2IsDirectParentOfNode1,
  node2IsAncestorOfNode1,
  moveNode,
  buildTreeMapAndSetParentsIds,
  prepareTreeBeforeSave
 };
