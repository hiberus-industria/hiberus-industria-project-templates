// Single source of truth for configuration.
// Reads EXCLUSIVELY from window.__RUNTIME_CONFIG__ and, if the key doesn't exist,
// uses a fallback that you define here.

type KnownKeys =
    | "DOCS_URL"
    | "OIDC_AUTHORITY"
    | "OIDC_CLIENT_ID"
    | "OIDC_PROFILE_URL";

type AppConfig = Partial<Record<KnownKeys, string>> & Record<string, unknown>;

const RUNTIME: Record<string, unknown> =
    (typeof window !== "undefined" && window.__RUNTIME_CONFIG__) || {};

export function getVar<T extends string = string>(
    key: string,
    fallback?: T,
): string | T | undefined {
    const val = RUNTIME?.[key];
    if (typeof val === "string") {
        const trimmed = val.trim();
        if (trimmed.length > 0) return trimmed;
    }
    return fallback;
}

let _cache: Readonly<AppConfig> | null = null;

export function getConfig(): Readonly<AppConfig> {
    if (_cache) return _cache;

    const cfg: AppConfig = {
        DOCS_URL: getVar("DOCS_URL", "http://localhost:3001/docs"),
        OIDC_AUTHORITY: getVar(
            "OIDC_AUTHORITY",
            "http://localhost:8080/realms/templates-aspire-react/",
        ),
        OIDC_CLIENT_ID: getVar("OIDC_CLIENT_ID", "frontend"),
        OIDC_PROFILE_URL: getVar(
            "OIDC_PROFILE_URL",
            "http://localhost:8080/realms/templates-aspire-react/account",
        ),
    };

    _cache = Object.freeze(cfg);
    return _cache;
}

export const docsUrl = () => getConfig().DOCS_URL as string;
export const authority = () => getConfig().OIDC_AUTHORITY as string;
export const clientId = () => getConfig().OIDC_CLIENT_ID as string;
export const profileUrl = () => getConfig().OIDC_PROFILE_URL as string;
