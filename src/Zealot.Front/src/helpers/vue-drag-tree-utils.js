/* eslint-disable */

const node2IsDirectParentOfNode1 = (node1, node2) => node1.parent.id === node2.id;

const fillParentsInTree = (node, parent, nodesMap) => {
  node.parent = parent;
  nodesMap.set(node.id, node);
  if(node.children) {
    node.children.forEach(child => {
      fillParentsInTree(child, node, nodesMap);
    });
  }
  return nodesMap;
};

const node2IsAncestorOfNode1 = function (node1, node2) {
  if (node1.parent === null) {
    // if from is the root node, return false right away
    return false;
  }

  let currentNode = node1;

  while(currentNode.parent) {
    if(currentNode.parent.id === node2.id) {
      return true;
    }
    currentNode = currentNode.parent;
  }

  return false;
};


const moveNode = (node1, node2) => {

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
  if (node2IsAncestorOfNode1(node2, node1)) {
    return;
  }

  // remove the moved node from its previous parent
  node1.parent.children = node1.parent.children.filter(
    item => item.id !== node1.id
  );
  // set node1's parent to node2
  node1.parent = node2;

  // add the moved node to the target node's children
  node2.children = [...node2.children, node1];
};

export {
 node2IsDirectParentOfNode1,
  node2IsAncestorOfNode1,
  moveNode,
  fillParentsInTree
 };
