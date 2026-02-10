import RequireRoles from "@/features/auth/require-roles";
import { createFileRoute, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/(dashboard)/dashboard")({
    component: RouteComponent,
    loader: () => ({
        crumb: "Dashboard",
    }),
});

function RouteComponent() {
    return (
        <RequireRoles roles={["operator"]}>
            <Outlet />
        </RequireRoles>
    );
}
