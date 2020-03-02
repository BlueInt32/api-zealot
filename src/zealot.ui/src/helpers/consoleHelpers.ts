const logsActive = process.env.DISPLAY_LOGS; // eslint-disable-line no-undef
const log = (...args: any[]) => {
  if (logsActive) {
    Reflect.apply(console.log, null, [...args]);
  }
};
const logAction = (actionName: string, ...args: any[]) => {
  log('[action]', actionName, ...args);
};
const logMutation = (actionName: string, ...args: any[]) => {
  log('[mutation]', actionName, ...args);
};

export { log, logAction, logMutation };
