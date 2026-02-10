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
import { HugeiconsIcon, type IconSvgElement } from "@hugeicons/react";
import { ArrowRight01Icon } from "@hugeicons/core-free-icons";
import type { Role } from "@/features/auth/roles";
import { Link } from "@tanstack/react-router";

export type NavMainItem = {
    title: string;
    url: string;
    icon: IconSvgElement;
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

export function NavMain({ items }: { items: NavMainItem[] }) {
    return (
        <SidebarGroup>
            <SidebarMenu>
                {items
                    .filter((item) => !item.hidden)
                    .map((item) => (
                        <Collapsible
                            key={item.title}
                            defaultOpen={item.isActive}
                            render={<SidebarMenuItem />}
                        >
                            <SidebarMenuButton
                                tooltip={item.title}
                                render={<Link to={item.url} />}
                            >
                                <HugeiconsIcon
                                    icon={item.icon}
                                    strokeWidth={2}
                                />
                                <span>{item.title}</span>
                            </SidebarMenuButton>
                            {item.items?.length ? (
                                <>
                                    <CollapsibleTrigger
                                        render={
                                            <SidebarMenuAction className="aria-expanded:rotate-90" />
                                        }
                                    >
                                        <HugeiconsIcon
                                            icon={ArrowRight01Icon}
                                            strokeWidth={2}
                                        />
                                        <span className="sr-only">Toggle</span>
                                    </CollapsibleTrigger>
                                    <CollapsibleContent>
                                        <SidebarMenuSub>
                                            {item.items?.map((subItem) => (
                                                <SidebarMenuSubItem
                                                    key={subItem.title}
                                                >
                                                    <SidebarMenuSubButton
                                                        render={
                                                            <Link
                                                                to={subItem.url}
                                                            />
                                                        }
                                                    >
                                                        <span>
                                                            {subItem.title}
                                                        </span>
                                                    </SidebarMenuSubButton>
                                                </SidebarMenuSubItem>
                                            ))}
                                        </SidebarMenuSub>
                                    </CollapsibleContent>
                                </>
                            ) : null}
                        </Collapsible>
                    ))}
            </SidebarMenu>
        </SidebarGroup>
    );
}
