import RequireRoles from "@/features/auth/require-roles";
import { createFileRoute, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/(dashboard)/config/users")({
    component: RouteComponent,
    loader: () => ({
        crumb: "Usuarios",
    }),
});

function RouteComponent() {
    return (
        <RequireRoles roles={["administrator"]}>
            <Outlet />
        </RequireRoles>
    );
}
