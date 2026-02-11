import React from "react";
import { useAuth } from "react-oidc-context";
import { useRoles } from "./use-roles";
import { Role } from "./roles";

type RequireRolesProps = {
    roles?: Role[];
    children: React.ReactNode;
};

function Forbidden() {
    return (
        <div className="flex flex-1 items-center justify-center space-x-4">
            <h1 className="text-2xl font-bold">403 - Acceso denegado</h1>
            <p className="text-gray-600">
                No tiene los permisos necesarios para ver esta página.
            </p>
        </div>
    );
}

export default function RequireRoles({
    roles = [],
    children,
}: RequireRolesProps) {
    const auth = useAuth();
    const { isAuthenticated, roles: currentRoles } = useRoles();

    if (auth.isLoading || !isAuthenticated) return null;

    const missingRequiredRoles =
        roles.length > 0 && !roles.every((r) => currentRoles.includes(r));

    if (missingRequiredRoles) return <Forbidden />;

    return <>{children}</>;
}
