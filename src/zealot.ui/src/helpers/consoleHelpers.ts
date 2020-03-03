const logsActive = process.env.VUE_APP_DISPLAY_LOGS === 'true'; // eslint-disable-line no-undef
export const log = (...args: any[]) => {
  if (logsActive) {
    Reflect.apply(console.log, null, [...args]);
  }
};
export const logAction = (actionName: string, ...args: any[]) => {
  log('[action]', actionName, ...args);
};
export const logArrow = (...args: any[]) => {
  log('→', ...args);
};

export const logMutation = (actionName: string, ...args: any[]) => {
  log('[mutation]', actionName, ...args);
};
