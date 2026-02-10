import { authority, clientId } from "@/global-variables";
import { UserManager, WebStorageStateStore } from "oidc-client-ts";

// Configuration for OIDC authentication
console.log(authority());
console.log(clientId());

export const userManager = new UserManager({
    authority: authority(),
    client_id: clientId(),
    redirect_uri: `${window.location.origin}${window.location.pathname}`,
    post_logout_redirect_uri: window.location.origin,
    userStore: new WebStorageStateStore({ store: window.sessionStorage }),
    monitorSession: true, // this allows cross tab login/logout detection
});

export const onSigninCallback = () => {
    window.history.replaceState({}, document.title, window.location.pathname);
};
