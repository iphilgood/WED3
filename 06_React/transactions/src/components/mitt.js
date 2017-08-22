// @flow

// Mitt: Tiny (~200b) functional event emitter / pubsub.
// Source: https://github.com/developit/mitt

type EventHandler = (event?: any) => void;
type EventHandlerList = Array<EventHandler>;
type EventHandlerMap = {
  [type: string]: EventHandlerList,
};

export default function mitt(all: EventHandlerMap) {
  all = all || Object.create(null);

  return {
    on(type: string, handler: EventHandler) {
      (all[type] || (all[type] = [])).push(handler);
    },

    off(type: string, handler: EventHandler) {
      if (all[type]) {
        all[type].splice(all[type].indexOf(handler) >>> 0, 1);
      }
    },

    emit(type: string, evt: any) {
      (all[type] || []).map((handler) => handler(evt));
      (all['*'] || []).map((handler) => handler(type, evt));
    }
  };
}
