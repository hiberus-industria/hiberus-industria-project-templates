import { ChevronRight, type LucideIcon } from "lucide-react";

import {
    Collapsible,
    CollapsibleContent,
    CollapsibleTrigger,
} from "@/shared/components/ui/collapsible";
import {
    SidebarGroup,
    SidebarMenu,
    SidebarMenuAction,
    SidebarMenuButton,
    SidebarMenuItem,
    SidebarMenuSub,
    SidebarMenuSubButton,
    SidebarMenuSubItem,
} from "@/shared/components/ui/sidebar";
import { Link, useRouterState } from "@tanstack/react-router";
import { Role } from "@/features/auth/roles";

export type NavMainItem = {
    title: string;
    url: string;
    icon: LucideIcon;
    isActive?: boolean;
    roles?: Role[];
    hidden?: boolean;
    items?: {
        title: string;
        url: string;
        roles?: Role[];
        hidden?: boolean;
    }[];
};

const startsWith = (path: string, base: string) =>
    path === base ||
    path.startsWith((base.endsWith("/") ? base.slice(0, -1) : base) + "/");

export function NavMain({ items }: { items: NavMainItem[] }) {
    const pathname = useRouterState({ select: (s) => s.location.pathname });

    return (
        <SidebarGroup>
            <SidebarMenu>
                {items
                    .filter((it) => !it.hidden)
                    .map((item) => {
                        const children =
                            item.items?.filter((c) => !c.hidden) ?? [];
                        const hasChildren = children.length > 0;
                        const parentActive = startsWith(pathname, item.url);
                        const childActive =
                            children.some((s) => startsWith(pathname, s.url)) ??
                            false;
                        const open = parentActive || childActive;

                        return (
                            <Collapsible key={item.title} asChild open={open}>
                                <SidebarMenuItem>
                                    <SidebarMenuButton
                                        asChild
                                        tooltip={item.title}
                                    >
                                        <Link
                                            to={item.url}
                                            activeOptions={{
                                                exact: hasChildren,
                                            }}
                                            activeProps={{
                                                "aria-current": "page",
                                                className:
                                                    "bg-sidebar-accent text-sidebar-accent-foreground",
                                            }}
                                        >
                                            <item.icon />
                                            <span>{item.title}</span>
                                        </Link>
                                    </SidebarMenuButton>
                                    {hasChildren ? (
                                        <>
                                            <CollapsibleTrigger asChild>
                                                <SidebarMenuAction className="data-[state=open]:rotate-90">
                                                    <ChevronRight />
                                                    <span className="sr-only">
                                                        Toggle
                                                    </span>
                                                </SidebarMenuAction>
                                            </CollapsibleTrigger>
                                            <CollapsibleContent>
                                                <SidebarMenuSub>
                                                    {children.map((subItem) => (
                                                        <SidebarMenuSubItem
                                                            key={subItem.title}
                                                        >
                                                            <SidebarMenuSubButton
                                                                asChild
                                                            >
                                                                <Link
                                                                    to={
                                                                        subItem.url
                                                                    }
                                                                    activeOptions={{
                                                                        exact: false,
                                                                    }}
                                                                    activeProps={{
                                                                        "aria-current":
                                                                            "page",
                                                                        className:
                                                                            "bg-sidebar-accent text-sidebar-accent-foreground",
                                                                    }}
                                                                >
                                                                    <span>
                                                                        {
                                                                            subItem.title
                                                                        }
                                                                    </span>
                                                                </Link>
                                                            </SidebarMenuSubButton>
                                                        </SidebarMenuSubItem>
                                                    ))}
                                                </SidebarMenuSub>
                                            </CollapsibleContent>
                                        </>
                                    ) : null}
                                </SidebarMenuItem>
                            </Collapsible>
                        );
                    })}
            </SidebarMenu>
        </SidebarGroup>
    );
}
