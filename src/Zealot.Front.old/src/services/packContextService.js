const packContexts = new Map();

const setPackContext = (key, context) => {
  packContexts.set(key, context);
};

const getPackContext = (key) => packContexts.get(key);

export {
  packContexts,
  setPackContext,
  getPackContext
};
