import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider } from "@tanstack/react-router";
import { QueryClientProvider } from "@tanstack/react-query";
import { NuqsAdapter } from "nuqs/adapters/react";
import { AuthProvider } from "react-oidc-context";

import "./styles.css";
import reportWebVitals from "./reportWebVitals.ts";
import { queryClient, router, setupApiClient } from "./config.ts";
import { onSigninCallback, userManager } from "./features/auth/oidc.ts";

setupApiClient();

// Render the app
const rootElement = document.getElementById("app");
if (rootElement && !rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <AuthProvider
                userManager={userManager}
                onSigninCallback={onSigninCallback}
            >
                <NuqsAdapter>
                    <QueryClientProvider client={queryClient}>
                        <RouterProvider router={router} />
                    </QueryClientProvider>
                </NuqsAdapter>
            </AuthProvider>
        </StrictMode>,
    );
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
