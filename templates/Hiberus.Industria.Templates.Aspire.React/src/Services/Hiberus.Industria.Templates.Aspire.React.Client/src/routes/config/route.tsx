import SidebarNav from "@/features/config/shared/components/sidebar-nav";
import { Outlet, createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/(dashboard)/config")({
    component: RouteComponent,
    loader: () => ({
        crumb: "Configuración",
    }),
});

function RouteComponent() {
    return (
        <div className="flex flex-1 flex-col lg:flex-row px-4 pb-4 gap-4">
            <SidebarNav />

            <Outlet />
        </div>
    );
}
