const logsActive = process.env.VUE_APP_DISPLAY_LOGS === 'true'; // eslint-disable-line no-undef
export const log = (...args: any) => {
  if (logsActive) {
    Reflect.apply(console.log, null, [...args]);
  }
};
export const logAction = (actionName: string, ...args: any) => {
  log(
    '%caction%c ' + actionName,
    'color: white;background: #F24D07;padding: 3px;border-radius: 2px;',
    'color: dark-grey;font-style: italic;',
    ...args
  );
};
export const logArrow = (...args: any[]) => {
  log('→', ...args);
};

export const logMutation = (mutationName: string, ...args: any[]) => {
  log(
    '%cmutation%c ' + mutationName,
    'color: white;background: #1CD2F6;padding: 3px;border-radius: 2px;',
    'color: dark-grey;',
    ...args
  );
};
