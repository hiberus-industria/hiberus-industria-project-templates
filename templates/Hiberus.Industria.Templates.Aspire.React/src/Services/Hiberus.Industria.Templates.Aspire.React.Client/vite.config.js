import { defineConfig, loadEnv } from "vite";
import viteReact from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

import tanstackRouter from "@tanstack/router-plugin/vite";
import { resolve } from "node:path";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), "");

    return {
        plugins: [
            tanstackRouter({ autoCodeSplitting: true }),
            viteReact(),
            tailwindcss(),
        ],
        test: {
            globals: true,
            environment: "jsdom",
        },
        resolve: {
            alias: {
                "@": resolve(__dirname, "./src"),
            },
        },
        server: {
            port: parseInt(env.PORT),
            proxy: {
                "/api": {
                    target:
                        process.env.services__server__https__0 ||
                        process.env.services__server__http__0,
                    changeOrigin: true,
                    secure: false,
                    ws: true,
                    rewrite: (path) => path.replace(/^\/api/, ""),
                },
            },
        },
    };
});
