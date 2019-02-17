/* eslint-disable */

const node2IsDirectParentOfNode1 = (node1, node2) => node1.$parent._uid === node2._uid;

const node2IsAncestorOfNode1 = function (from, to) {
  const rootNodeName = 'vue-drag-tree';

  if(from.$options._componentTag === rootNodeName) {
    // if from is the root node, return false right away
    return false;
  }
  let parent = from.$parent;

  let ok = false;
  let status = false;
  while (!ok) {
    if (parent._uid === to._uid) {
      ok = true;
      status = true;
      continue;
    }
    if (
      !parent.$options._componentTag
      || parent.$options._componentTag === rootNodeName
    ) {
      ok = true;
      break;
    }
    parent = parent.$parent;
  }

  return status;
};

const allowDrop = (targetModel) => {
  return targetModel.type === 'pack';
};

const exchangeData = (from, to) => {

  // Having "P(S)" = direct Parent of S, and "X ∈ Y" = X is a descendant of Y
  // Prevent-drop rules : for "Source S droping on Target T", the following structures
  // will prevent the drop to occur
  // 1/ S == T, cannot drop a node in itself
  // 2/ P(S) == T, cannot drop a node where it sits already
  // 3/ T ∈ S, cannot drop a node in one of its children

  // Rule 1: if S == T, block because one cannot drop a node in itself
  if (from._uid === to._uid) {
    return;
  }

  // Rule 2: if P(S) == T, block because one cannot drop a node where it sits already
  if (node2IsDirectParentOfNode1(from, to)) {
    return;
  }

  // Rule 3: if 'T ∈ S' (or 'S is ancestor of T'), block because one cannot drop a node into one of its descendant
  if (node2IsAncestorOfNode1(to, from)) {
    return;
  }

  const fromModelCopy = { ...from.model };

  // Remove the moved node from its previous parent
  from.$parent.model.children = from.$parent.model.children.filter(
    item => item.id !== fromModelCopy.id
  );

  // Add the moved node to the target node's children
  to.model.children = [...to.model.children, fromModelCopy];
  // if (to.model.children) {
  //   to.model.children = to.model.children.concat([fromModelCopy]);
  // } else {
  //   to.model.children = [fromModelCopy];
  // }
};

export { node2IsDirectParentOfNode1,
  node2IsAncestorOfNode1,
  exchangeData,
  allowDrop
 };
