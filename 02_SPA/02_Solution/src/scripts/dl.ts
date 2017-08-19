import { CounterModel } from './bl';
import { container } from "./di";

export interface ICounterDataResource {
  get(callback: (data: any) => void): void;
  sendUp(callback: (data: any) => void): void;
}

export class CounterDataResource implements ICounterDataResource {
  get(callback: (data: any) => void): void {
    $.get('/api', (data) => {
      callback(data);
    });
  }

  sendUp(callback: (data: any) => void): void {
    $.post('/api/up', (data) => {
      callback(data);
    });
  }
}

container.register("CounterDataResource", [], CounterDataResource);
