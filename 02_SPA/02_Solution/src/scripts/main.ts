import { Router, ICounterController } from "./ui";
import { container } from "./di";

$(function () {
  const routerOutlet = $("#appContainer");
  const router = new Router({
    rootPath: "/",
    initialRoute: "index",
    routes: {
      "index": () => { container.resolve<ICounterController>("CounterController").indexAction(routerOutlet); }
    }
  });

  router.initialize();
});
