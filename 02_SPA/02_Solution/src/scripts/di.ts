/**
 * Dependency Injection
 */

interface Registration<T> {
  name: string;
  dependencies: string[];
  type: new (...args: any[]) => T;
  instance: any;
}

class Container {
  private registrations: { [key: string]: Registration<any>} = { };

  constructor() { }

  register<T>(name: string, dependencies: string[], type: new (...args: any[]) => T): void {
    this.registrations[name] = { name, dependencies, type, instance: null }
  }

  resolve<T>(name: string): T {
    if (!this.registrations[name].instance) {
      this.registrations[name].instance = new this.registrations[name].type(...this.resolveDependencies(name));
    }

    return this.registrations[name].instance;
  }

  resolveDependencies(name: string): any[] {
    const dependencies = [];
    if (this.registrations[name].dependencies) {
      this.registrations[name].dependencies.forEach((dependency) => {
        dependencies.push(this.resolve<any>(dependency));
      })
    }

    return dependencies;
  }
}

export const container = new Container();
