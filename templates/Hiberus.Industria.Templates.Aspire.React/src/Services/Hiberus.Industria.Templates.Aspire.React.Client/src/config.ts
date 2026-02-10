import { routeTree } from "./routeTree.gen";
import { createRouter } from "@tanstack/react-router";

// Configuration for routing
export const router = createRouter({
    routeTree,
    context: {},
    defaultPreload: "intent",
    scrollRestoration: true,
    defaultStructuralSharing: true,
    defaultPreloadStaleTime: 0,
});

declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}
