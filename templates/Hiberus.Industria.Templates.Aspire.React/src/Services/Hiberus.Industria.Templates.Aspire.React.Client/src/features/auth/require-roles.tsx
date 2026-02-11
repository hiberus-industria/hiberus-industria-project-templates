import React from "react";
import { useAuth } from "react-oidc-context";
import { useRoles } from "@/features/auth/use-roles";
import { Role } from "@/features/auth/roles";
import {
    Empty,
    EmptyContent,
    EmptyDescription,
    EmptyHeader,
    EmptyTitle,
} from "@/shared/components/ui/empty";
import { Button } from "@/shared/components/ui/button";
import { Link } from "@tanstack/react-router";

type RequireRolesProps = {
    roles?: Role[];
    children: React.ReactNode;
};

function Forbidden() {
    return (
        <main className="flex flex-1 h-screen w-full items-center justify-center p-4">
            <Empty>
                <EmptyHeader>
                    <EmptyTitle>403 - Acceso denegado</EmptyTitle>
                    <EmptyDescription>
                        No tiene los permisos necesarios para ver esta página.
                    </EmptyDescription>
                </EmptyHeader>
                <EmptyContent>
                    <Button variant="link">
                        <Link to="/">Volver al inicio</Link>
                    </Button>
                </EmptyContent>
            </Empty>
        </main>
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
