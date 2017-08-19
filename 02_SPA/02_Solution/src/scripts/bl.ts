import { ICounterDataResource } from './dl';
import { container } from "./di";

/**
 * Interface
 */
 export interface ICounterService {
   load(callback: (data: CounterModel) => void): void;
   up(callback: (data: CounterModel) => void): void;
 }

/**
 * Model
 */
export class CounterModel {
  private team: string;
  private count: number;

  constructor(team: string, count: number) {
    this.team = team || "unspecified";
    this.count = count || 0;
  }

  static fromDto(dto: any): CounterModel {
    return new CounterModel(dto.team, dto.count);
  }
}

/**
 * Service
 */
export class CounterService implements ICounterService {
  private counterDataResource: ICounterDataResource;

  constructor(counterDataResource: ICounterDataResource) {
    this.counterDataResource = counterDataResource;
  }

  load(callback: (data: CounterModel) => any): void {
    this.counterDataResource.get((dto) => {
      callback(CounterModel.fromDto(dto));
    });
  }

  up(callback: (data: CounterModel) => any): void {
    this.counterDataResource.sendUp((dto) => {
      callback(CounterModel.fromDto(dto));
    });
  }
}

container.register("CounterService", ["CounterDataResource"], CounterService);
