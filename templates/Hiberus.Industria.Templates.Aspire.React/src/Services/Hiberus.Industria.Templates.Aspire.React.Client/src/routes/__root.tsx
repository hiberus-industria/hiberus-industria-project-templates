import { AppSidebar } from "@/shared/components/layout/app-sidebar";
import ProtectedApp from "@/shared/components/protected-app";
import TanstackBreadcrumb from "@/shared/components/tanstack-breadcrumb";
import { ThemeProvider } from "@/shared/components/theme-provider";
import { Button } from "@/shared/components/ui/button";
import {
    Empty,
    EmptyHeader,
    EmptyTitle,
    EmptyDescription,
    EmptyContent,
} from "@/shared/components/ui/empty";
import { Separator } from "@/shared/components/ui/separator";
import {
    SidebarInset,
    SidebarProvider,
    SidebarTrigger,
} from "@/shared/components/ui/sidebar";
import { Toaster } from "@/shared/components/ui/sonner";
import { Link, Outlet, createRootRoute } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

export const Route = createRootRoute({
    component: () => (
        <ProtectedApp>
            <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
                <SidebarProvider>
                    <AppSidebar />
                    <SidebarInset>
                        <header className="flex h-16 shrink-0 items-center gap-2">
                            <div className="flex items-center gap-2 px-4">
                                <SidebarTrigger className="-ml-1" />
                                <Separator
                                    orientation="vertical"
                                    className="me-2 data-[orientation=vertical]:h-4"
                                />
                                <TanstackBreadcrumb />
                            </div>
                        </header>

                        <Outlet />
                    </SidebarInset>
                </SidebarProvider>
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
