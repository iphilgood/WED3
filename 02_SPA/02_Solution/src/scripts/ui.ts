import { ICounterService, CounterModel } from "./bl";
import { container } from "./di";

/**
 * Routing
 */
export interface RouteConfig {
  rootPath: string;
  initialRoute: string;
  routes: { [key: string]: Function }

}
export class Router {
  private routeConfig: RouteConfig;
  /**
   * @param routeConfig Supported format:
   * {
   *  rootPath: String,
   *  initialRoute: String,
   *  routes: { [String]: Function }
   * }
   */
  constructor(routeConfig: RouteConfig) {
    this.routeConfig = routeConfig;
  }

  navigate(route): void {
    window.history.pushState(null, void 0, route);
    this.activate(route);
  }

  activate(route): void {
    if (this.routeConfig.routes[route]) {
      this.routeConfig.routes[route]();
    }
  }

  initialize(): void {
    let activatedRoute = self.location.pathname;
    if (self.location.pathname.indexOf(this.routeConfig.rootPath) == 0) {
      activatedRoute = self.location.pathname.substring(this.routeConfig.rootPath.length);
    }

    this.activate(activatedRoute || this.routeConfig.initialRoute);
  }
}

/**
 * MVC
 */
export interface ICounterController {
  indexAction(viweRef): void;
}

export class CounterController implements ICounterController {
  private counterService: ICounterService;
  private indexTemplateCompiled: HandlebarsTemplateDelegate;

  constructor(counterService: ICounterService) {
    this.counterService = counterService;
    this.indexTemplateCompiled = Handlebars.compile($('#index-view').html());
  }

  indexAction(viewRef): void {
    this.counterService.load((data: CounterModel) => {
      this.renderIndexView(viewRef, data);
    });

    $(viewRef).on('click', '[data-click=up]', (e) => {
      this.counterService.up((data: CounterModel) => {
        this.renderIndexView(viewRef, data);
      });

      e.preventDefault();
    });
  }

  renderIndexView(viewRef, model: CounterModel): void {
    viewRef.html(this.indexTemplateCompiled({
      counter: model
    }));
  }
}

container.register("CounterController", ["CounterService"], CounterController);
