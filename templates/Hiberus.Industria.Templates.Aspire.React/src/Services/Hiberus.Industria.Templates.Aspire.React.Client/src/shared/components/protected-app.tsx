import { client } from "@/client/client.gen";
import { useEffect, useState } from "react";
import { useAuth, useAutoSignin } from "react-oidc-context";

type ProtectedAppProps = {
    children: React.ReactNode;
};

export default function ProtectedApp({ children }: ProtectedAppProps) {
    const auth = useAuth();

    const { isLoading, isAuthenticated, error } = useAutoSignin({
        signinMethod: "signinRedirect",
    });

    const [isInterceptorReady, setIsInterceptorReady] = useState(false);

    /**
     * Inject the authorization header into requests.
     *
     * See {@link https://heyapi.dev/openapi-ts/clients/fetch#interceptors}
     */
    useEffect(() => {
        setIsInterceptorReady(false);

        const user = auth.user;
        const bearerToken = user?.access_token;
        const tokenType = user?.token_type ?? "Bearer";

        if (!bearerToken) return;

        const interceptorId = client.interceptors.request.use((request) => {
            request.headers.set("Authorization", `${tokenType} ${bearerToken}`);
            return request;
        });

        setIsInterceptorReady(true);

        return () => client.interceptors.request.eject(interceptorId);
    }, [auth.user?.access_token]);

    const anyLoading = isLoading || !isInterceptorReady;
    const anyErrorMessage = error?.message;

    if (anyLoading || anyErrorMessage || !isAuthenticated) return null;

    return <>{children}</>;
}
