import * as React from "react";

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
import { HugeiconsIcon } from "@hugeicons/react";
import {
    CommandIcon,
    LayoutGrid,
    Settings,
    LifeBuoy,
    Book,
} from "@hugeicons/core-free-icons";
import { docsUrl } from "@/global-variables";
import { Link } from "@tanstack/react-router";

const dataNew: { navMain: NavMainItem[]; navSecondary: NavSecondaryItem[] } = {
    navMain: [
        {
            title: "Dashboard",
            url: "/dashboard",
            icon: LayoutGrid,
            roles: ["administrator"],
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

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
    return (
        <Sidebar variant="inset" {...props}>
            <SidebarHeader>
                <SidebarMenu>
                    <SidebarMenuItem>
                        <SidebarMenuButton
                            size="lg"
                            render={<Link to="/dashboard" />}
                        >
                            <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                                <HugeiconsIcon
                                    icon={CommandIcon}
                                    strokeWidth={2}
                                    className="size-4"
                                />
                            </div>
                            <div className="grid flex-1 text-left text-sm leading-tight">
                                <span className="truncate font-medium">
                                    Hiberus industria
                                </span>
                                <span className="truncate text-xs">
                                    Aspire React Template
                                </span>
                            </div>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                </SidebarMenu>
            </SidebarHeader>
            <SidebarContent>
                <NavMain items={dataNew.navMain} />
                <NavSecondary
                    items={dataNew.navSecondary}
                    className="mt-auto"
                />
            </SidebarContent>
            <SidebarFooter>
                <NavUser />
            </SidebarFooter>
        </Sidebar>
    );
}
