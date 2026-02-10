import { StrictMode } from "react";
import ReactDOM from "react-dom/client";

import "./index.css";
import { queryClient, router, setupApiClient } from "./config.ts";
import { AuthProvider } from "react-oidc-context";
import { onSigninCallback, userManager } from "./features/auth/oidc.ts";
import { RouterProvider } from "@tanstack/react-router";
import reportWebVitals from "./report-web-vitals.ts";
import { QueryClientProvider } from "@tanstack/react-query";

setupApiClient();

const rootElement = document.getElementById("app");
if (rootElement && !rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <AuthProvider
                userManager={userManager}
                onSigninCallback={onSigninCallback}
            >
                <QueryClientProvider client={queryClient}>
                    <RouterProvider router={router} />
                </QueryClientProvider>
            </AuthProvider>
        </StrictMode>,
    );
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
