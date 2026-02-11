import { createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/(dashboard)/config/")({
    loader: () => redirect({ to: "/config/users" }),
});
