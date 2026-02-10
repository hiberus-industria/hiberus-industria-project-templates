import path from "path";
import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";
import { defineConfig, loadEnv } from "vite";

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), "");
    return {
        plugins: [react(), tailwindcss()],
        resolve: {
            alias: {
                "@": path.resolve(__dirname, "./src"),
            },
        },
        server: {
            port: parseInt(env.PORT),
            proxy: {
                // Proxy API calls to the app service
                "/api": {
                    target: process.env.SERVER_HTTPS || process.env.SERVER_HTTP,
                    changeOrigin: true,
                },
            },
        },
    };
});
