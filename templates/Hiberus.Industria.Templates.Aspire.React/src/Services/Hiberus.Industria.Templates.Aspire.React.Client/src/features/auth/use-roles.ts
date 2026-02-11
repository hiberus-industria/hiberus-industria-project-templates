import { useAuth } from "react-oidc-context";
import { getRealmRoles } from "./jwt";
import { hasAll, hasAny, hasOne, Role } from "./roles";

export function useRoles() {
    const auth = useAuth();
    const isAuthenticated =
        auth.isAuthenticated && !auth.isLoading && !!auth.user && !auth.error;

    const accessToken = auth.user?.access_token ?? null;
    const currentRoles = getRealmRoles(accessToken);

    return {
        isAuthenticated,
        roles: currentRoles,
        hasRole: (role: Role) => hasOne(currentRoles, role),
        hasAny: (roles: Role[] | Role) =>
            hasAny(currentRoles, Array.isArray(roles) ? roles : [roles]),
        hasAll: (roles: Role[] | Role) =>
            hasAll(currentRoles, Array.isArray(roles) ? roles : [roles]),
    };
}
