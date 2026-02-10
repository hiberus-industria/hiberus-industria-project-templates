import { routeTree } from "./routeTree.gen";
import { client } from "./client/client.gen";
import { createRouter } from "@tanstack/react-router";
import { QueryClient } from "@tanstack/react-query";

// Configuration for API client
export const queryClient = new QueryClient();

export const setupApiClient = () => {
    client.setConfig({
        baseUrl: "/api",
    });
};

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
