import { JSX } from "react";

export {};

declare global {
    interface Window {
        __RUNTIME_CONFIG__?: RuntimeRuntimeConfig;
    }

    interface RuntimeRuntimeConfig {
        DOCS_URL?: string;
        OIDC_AUTHORITY?: string;
        OIDC_CLIENT_ID?: string;
        OIDC_PROFILE_URL?: string;
        [key: string]: unknown;
    }

    namespace JSX {
        interface Element extends React.JSX.Element {}
    }
}
