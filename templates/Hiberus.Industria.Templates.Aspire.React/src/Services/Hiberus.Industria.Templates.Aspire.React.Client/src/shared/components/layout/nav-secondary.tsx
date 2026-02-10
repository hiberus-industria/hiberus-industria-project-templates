import * as React from "react";
import { HugeiconsIcon, type IconSvgElement } from "@hugeicons/react";

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
    icon: IconSvgElement;
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
                            <SidebarMenuButton size="sm">
                                <Link
                                    to={item.url}
                                    target={
                                        item.openInNewTab ? "_blank" : undefined
                                    }
                                >
                                    <HugeiconsIcon
                                        icon={item.icon}
                                        strokeWidth={2}
                                    />
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
