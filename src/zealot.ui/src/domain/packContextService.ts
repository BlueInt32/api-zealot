const packContexts = new Map();

const setPackContext = (key: string, context: any) => {
  packContexts.set(key, context);
};

const getPackContext = (key: string) => packContexts.get(key);

export { packContexts, setPackContext, getPackContext };
