/* eslint-disable */

const findRoot = which => {
  let ok = false;
  let that = which;
  while (!ok) {
    // 根据组件name来判断
    if (that.$options._componentTag === 'vue-drag-tree') {
      ok = true;
      // 交换两者的数据
      break;
    }
    that = that.$parent;
  }
  return that;
};

const hasInclude = (from, to) => from.$parent._uid === to._uid;

const isLinealRelation = function (from, to) {
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
      || parent.$options._componentTag === 'vue-drag-tree'
    ) {
      ok = true;
      break;
    }
    parent = parent.$parent;
  }

  return status;
};

const exchangeData = (rootCom, from, to) => {
  // 如果拖动节点和被拖动节点相同，return;
  if (from._uid === to._uid) {
    return;
  }

  // 如果两者是父子关系且from是父节点，to是子节点，什么都不做
  if (hasInclude(to, from)) {
    return;
  }

  const newFrom = { ...from.model };

  // 如果两者是父子关系。将from节点，移动到to节点一级且放到其后一位
  if (hasInclude(from, to)) {
    // 如果“父”是最上层节点（节点数组中的最外层数据）
    const tempParent = to.$parent;
    const toModel = to.model;

    if (tempParent.$options._componentTag === 'vue-drag-tree') {
      // 将from节点添加到 根数组中
      tempParent.newData.push(newFrom);
      // 移除to中from节点信息；
      toModel.children = toModel.children.filter(item => item.id !== newFrom.id);
      return;
    }

    const toParentModel = tempParent.model;
    // 先移除to中from节点信息；
    toModel.children = toModel.children.filter(item => item.id !== newFrom.id);
    // 将 from节点 添加到 to 一级别中。
    toParentModel.children = toParentModel.children.concat([newFrom]);
    return;
  }

  // 如果是 线性 关系，但非父子
  if (isLinealRelation(from, to)) {
    const fromParentModel = from.$parent.model;
    const toModel = to.model;

    // 先将from从其父节点信息移除；
    fromParentModel.children = fromParentModel.children.filter(
      item => item.id !== newFrom.id
    );

    // 再from节点添加到to节点中最后一位。
    toModel.children = toModel.children.concat([newFrom]);
    return;
  }

  // 两节点（无线性关系），把from节点扔到to节点中。
  const fromParentModel = from.$parent.model;
  const toModel = to.model;
  // 先将from从其父节点信息移除；
  if (from.$parent.$options._componentTag === 'vue-drag-tree') {

    from.$parent.newData = from.$parent.newData.filter(
      item => item.id !== newFrom.id
    );
  } else {
    fromParentModel.children = fromParentModel.children.filter(
      item => item.id !== newFrom.id
    );
  }

  if (toModel.children) {
    toModel.children = toModel.children.concat([newFrom]);
  } else {
    toModel.children = [newFrom];
  }
};

export { findRoot, exchangeData };
