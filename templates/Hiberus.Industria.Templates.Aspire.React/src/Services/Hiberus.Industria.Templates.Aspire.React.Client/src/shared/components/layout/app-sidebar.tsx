import * as React from "react";
import {
    Book,
    LifeBuoy,
    Settings,
    LayoutGrid,
} from "@hugeicons/core-free-icons";

import { NavMain, type NavMainItem } from "@/shared/components/layout/nav-main";
import {
    NavSecondary,
    type NavSecondaryItem,
} from "@/shared/components/layout/nav-secondary";
import { NavUser } from "@/shared/components/layout/nav-user";
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
import { type Role } from "@/features/auth/roles";

const data: { navMain: NavMainItem[]; navSecondary: NavSecondaryItem[] } = {
    navMain: [
        {
            title: "Dashboard",
            url: "/dashboard",
            icon: LayoutGrid,
            roles: ["operator"],
        },
        {
            title: "Configuración",
            url: "/config",
            icon: Settings,
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
            url: "https://soporte.hiberus.com/",
            icon: LifeBuoy,
            openInNewTab: false,
        },
        {
            title: "Documentación",
            url: docsUrl(),
            icon: Book,
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
                        <SidebarMenuButton size="lg">
                            <Link to="/dashboard">
                                <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                                    <img
                                        src="/logo.svg"
                                        alt="Application logo"
                                        className="size-6 invert"
                                    />
                                </div>
                                <div className="grid flex-1 text-left text-sm leading-tight">
                                    <span className="truncate font-medium">
                                        Hiberus
                                    </span>
                                    <span className="truncate text-xs">
                                        Industria
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
