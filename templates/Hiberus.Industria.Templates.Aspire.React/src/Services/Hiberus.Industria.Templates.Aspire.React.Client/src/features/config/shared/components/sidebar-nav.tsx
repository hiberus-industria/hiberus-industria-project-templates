import {
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from "@/shared/components/ui/sidebar";
import { Link } from "@tanstack/react-router";

const data = [
    {
        title: "Usuarios",
        href: "/config/users",
    },
];

export default function SidebarNav() {
    return (
        <aside>
            <SidebarMenu>
                {data.map((item, index) => (
                    <SidebarMenuItem key={index}>
                        <SidebarMenuButton asChild tooltip={item.title}>
                            <Link
                                to={item.href}
                                activeOptions={{ exact: false }}
                                activeProps={{
                                    "aria-current": "page",
                                    className:
                                        "bg-sidebar-accent text-sidebar-accent-foreground",
                                }}
                            >
                                <span>{item.title}</span>
                            </Link>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                ))}
            </SidebarMenu>
        </aside>
    );
}
