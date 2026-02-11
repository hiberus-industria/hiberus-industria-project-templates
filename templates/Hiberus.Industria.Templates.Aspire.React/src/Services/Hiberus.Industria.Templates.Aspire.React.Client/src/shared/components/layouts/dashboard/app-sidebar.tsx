import * as React from "react";
import {
    BookIcon,
    LifeBuoy,
    Settings2Icon,
    LayoutDashboardIcon,
} from "lucide-react";

import {
    NavMain,
    NavMainItem,
} from "@/shared/components/layouts/dashboard/nav-main";
import {
    NavSecondary,
    NavSecondaryItem,
} from "@/shared/components/layouts/dashboard/nav-secondary";
import { NavUser } from "@/shared/components/layouts/dashboard/nav-user";
import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarHeader,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from "@/shared/components/ui/sidebar";
import { docsUrl } from "@/global-variables";
import { Link } from "@tanstack/react-router";
import { useRoles } from "@/features/auth/use-roles";
import { Role } from "@/features/auth/roles";

const data: { navMain: NavMainItem[]; navSecondary: NavSecondaryItem[] } = {
    navMain: [
        {
            title: "Dashboard",
            url: "/dashboard",
            icon: LayoutDashboardIcon,
            roles: ["administrator"],
        },
        {
            title: "Configuración",
            url: "/config",
            icon: Settings2Icon,
            roles: ["administrator"],
            items: [
                {
                    title: "Usuarios",
                    url: "/config/users",
                    roles: ["administrator"],
                },
            ],
        },
    ],
    navSecondary: [
        {
            title: "Soporte",
            url: "https://soporte.hiberus.com",
            icon: LifeBuoy,
            openInNewTab: true,
        },
        {
            title: "Documentación",
            url: docsUrl(),
            icon: BookIcon,
            openInNewTab: true,
        },
    ],
};

const filterByRoles = (
    items: NavMainItem[],
    hasAny: (roles: Role[] | Role) => boolean,
) => items.filter((item) => !item.roles || hasAny(item.roles));

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
    const { hasAny } = useRoles();

    const filteredMain = React.useMemo(
        () => filterByRoles(data.navMain, hasAny),
        [hasAny],
    );
    const filteredSecondary = React.useMemo(
        () => filterByRoles(data.navSecondary, hasAny),
        [hasAny],
    );

    return (
        <Sidebar variant="inset" {...props}>
            <SidebarHeader>
                <SidebarMenu>
                    <SidebarMenuItem>
                        <SidebarMenuButton size="lg" asChild>
                            <Link to="/dashboard">
                                <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                                    <img
                                        src="/logo.svg"
                                        alt="Volkswagen logo"
                                        className="size-6 invert"
                                    />
                                </div>
                                <div className="grid flex-1 text-left text-sm leading-tight">
                                    <span className="truncate font-medium">
                                        Hiberus industria
                                    </span>
                                    <span className="truncate text-xs">
                                        Templates Aspire React
                                    </span>
                                </div>
                            </Link>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                </SidebarMenu>
            </SidebarHeader>
            <SidebarContent>
                <NavMain items={filteredMain} />
                <NavSecondary items={filteredSecondary} className="mt-auto" />
            </SidebarContent>
            <SidebarFooter>
                <NavUser />
            </SidebarFooter>
        </Sidebar>
    );
}
