export type Decoded = { realm_access?: { roles?: string[] } };

export function decodeJwt(token?: string | null): Decoded | null {
    if (!token) return null;
    try {
        const b64 = token.split(".")[1]?.replace(/-/g, "+").replace(/_/g, "/");
        if (!b64) return null;
        const json = atob(b64);
        return JSON.parse(json);
    } catch {
        return null;
    }
}

export function getRealmRoles(accessToken?: string | null): string[] {
    return decodeJwt(accessToken)?.realm_access?.roles ?? [];
}
