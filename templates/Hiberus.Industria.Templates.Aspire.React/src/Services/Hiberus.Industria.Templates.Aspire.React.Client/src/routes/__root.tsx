import ProtectedApp from "@/shared/components/protected-app";
import { ThemeProvider } from "@/shared/components/theme-provider";
import { Button } from "@/shared/components/ui/button";
import {
    Empty,
    EmptyHeader,
    EmptyTitle,
    EmptyDescription,
    EmptyContent,
} from "@/shared/components/ui/empty";
import { Toaster } from "@/shared/components/ui/sonner";
import { Link, Outlet, createRootRoute } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

export const Route = createRootRoute({
    component: () => (
        <ProtectedApp>
            <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
                <Outlet />
            </ThemeProvider>

            <Toaster />
            <TanStackRouterDevtools position="bottom-right" />
        </ProtectedApp>
    ),
    notFoundComponent: () => (
        <main className="flex flex-1 h-screen w-full items-center justify-center p-4">
            <Empty>
                <EmptyHeader>
                    <EmptyTitle>404 - Página no encontrada</EmptyTitle>
                    <EmptyDescription>
                        La página que está buscando no existe o ha sido movida.
                    </EmptyDescription>
                </EmptyHeader>
                <EmptyContent>
                    <Button variant="link">
                        <Link to="/">Volver al inicio</Link>
                    </Button>
                </EmptyContent>
            </Empty>
        </main>
    ),
    errorComponent: () => (
        <main className="flex flex-1 h-screen w-full items-center justify-center p-4">
            <Empty>
                <EmptyHeader>
                    <EmptyTitle>Error</EmptyTitle>
                    <EmptyDescription>
                        Ha ocurrido un error inesperado. Por favor, inténtelo de
                        nuevo más tarde.
                    </EmptyDescription>
                </EmptyHeader>
                <EmptyContent>
                    <Button variant="link">
                        <Link to="/">Volver al inicio</Link>
                    </Button>
                </EmptyContent>
            </Empty>
        </main>
    ),
});
