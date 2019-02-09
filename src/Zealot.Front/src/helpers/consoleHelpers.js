const logsActive = __DISPLAY_LOGS__; // eslint-disable-line no-undef
const log = (...args) => {
  if (logsActive) {
    Reflect.apply(console.log, null, [...args]);
  }
};
const logAction = (actionName, ...args) => {
  log('[action]', actionName, ...args);
};
const logMutation = (actionName, ...args) => {
  log('[mutation]', actionName, ...args);
};

export {
  log,
  logAction,
  logMutation
};
