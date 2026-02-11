import * as React from "react";
import { type LucideIcon } from "lucide-react";

import {
    SidebarGroup,
    SidebarGroupContent,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from "@/shared/components/ui/sidebar";
import { Link } from "@tanstack/react-router";

export type NavSecondaryItem = {
    title: string;
    url: string;
    icon: LucideIcon;
    openInNewTab?: boolean;
};

export function NavSecondary({
    items,
    ...props
}: {
    items: NavSecondaryItem[];
} & React.ComponentPropsWithoutRef<typeof SidebarGroup>) {
    return (
        <SidebarGroup {...props}>
            <SidebarGroupContent>
                <SidebarMenu>
                    {items.map((item) => (
                        <SidebarMenuItem key={item.title}>
                            <SidebarMenuButton asChild size="sm">
                                <Link
                                    to={item.url}
                                    target={
                                        item.openInNewTab ? "_blank" : undefined
                                    }
                                >
                                    <item.icon />
                                    <span>{item.title}</span>
                                </Link>
                            </SidebarMenuButton>
                        </SidebarMenuItem>
                    ))}
                </SidebarMenu>
            </SidebarGroupContent>
        </SidebarGroup>
    );
}
