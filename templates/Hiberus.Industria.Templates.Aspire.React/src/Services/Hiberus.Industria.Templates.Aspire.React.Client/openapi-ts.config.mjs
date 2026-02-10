/** @type {import('@hey-api/openapi-ts').UserConfig} */
export default {
    input: {
        path: "http://localhost:5211/openapi/v1.json",
    },
    output: {
        path: "src/client",
    },
    plugins: ["@hey-api/client-fetch", "@tanstack/react-query"],
};
