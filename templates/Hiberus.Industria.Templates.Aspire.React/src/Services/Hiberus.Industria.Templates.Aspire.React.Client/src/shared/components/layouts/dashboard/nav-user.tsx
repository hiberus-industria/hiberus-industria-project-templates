import {
    ChevronsUpDown,
    LogOutIcon,
    MonitorCogIcon,
    MoonIcon,
    SunIcon,
    UserIcon,
} from "lucide-react";

import {
    Avatar,
    AvatarFallback,
    AvatarImage,
} from "@/shared/components/ui/avatar";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuGroup,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuPortal,
    DropdownMenuSeparator,
    DropdownMenuSub,
    DropdownMenuSubContent,
    DropdownMenuSubTrigger,
    DropdownMenuTrigger,
} from "@/shared/components/ui/dropdown-menu";
import {
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
    useSidebar,
} from "@/shared/components/ui/sidebar";
import { useTheme, type Theme } from "@/shared/components/theme-provider";
import { useAuth } from "react-oidc-context";
import { getInitialsFromFullName } from "@/shared/lib/utils";
import { Link } from "@tanstack/react-router";
import { profileUrl } from "@/global-variables";

export function NavUser() {
    const auth = useAuth();
    const { isMobile } = useSidebar();
    const { theme, setTheme } = useTheme();

    const user = auth.user?.profile;

    const onThemeChange = (
        event: React.MouseEvent<HTMLDivElement, MouseEvent>,
        newTheme: Theme,
    ) => {
        event.preventDefault();
        setTheme(newTheme);
    };

    return (
        <SidebarMenu>
            <SidebarMenuItem>
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <SidebarMenuButton
                            size="lg"
                            className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
                        >
                            <Avatar className="h-8 w-8 rounded-lg">
                                <AvatarImage
                                    src={user?.picture}
                                    alt="Foto de perfil"
                                />
                                <AvatarFallback className="rounded-lg">
                                    {getInitialsFromFullName(user?.name)}
                                </AvatarFallback>
                            </Avatar>
                            <div className="grid flex-1 text-left text-sm leading-tight">
                                <span className="truncate font-medium">
                                    {user?.name}
                                </span>
                                <span className="truncate text-xs">
                                    {user?.email}
                                </span>
                            </div>
                            <ChevronsUpDown className="ml-auto size-4" />
                        </SidebarMenuButton>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent
                        className="w-(--radix-dropdown-menu-trigger-width) min-w-56 rounded-lg"
                        side={isMobile ? "bottom" : "right"}
                        align="end"
                        sideOffset={4}
                    >
                        <DropdownMenuLabel className="p-0 font-normal">
                            <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
                                <Avatar className="h-8 w-8 rounded-lg">
                                    <AvatarImage
                                        src={user?.picture}
                                        alt={user?.name}
                                    />
                                    <AvatarFallback className="rounded-lg">
                                        {getInitialsFromFullName(user?.name)}
                                    </AvatarFallback>
                                </Avatar>
                                <div className="grid flex-1 text-left text-sm leading-tight">
                                    <span className="truncate font-medium">
                                        {user?.name}
                                    </span>
                                    <span className="truncate text-xs">
                                        {user?.email}
                                    </span>
                                </div>
                            </div>
                        </DropdownMenuLabel>
                        <DropdownMenuSeparator />
                        <DropdownMenuGroup>
                            <DropdownMenuItem asChild>
                                <Link to={profileUrl()} target="_blank">
                                    <UserIcon />
                                    Perfil
                                </Link>
                            </DropdownMenuItem>
                            <DropdownMenuSub>
                                <DropdownMenuSubTrigger>
                                    <div className="flex items-center gap-2">
                                        {
                                            {
                                                light: (
                                                    <SunIcon className="h-4 w-4" />
                                                ),
                                                dark: (
                                                    <MoonIcon className="h-4 w-4" />
                                                ),
                                                system: (
                                                    <MonitorCogIcon className="h-4 w-4" />
                                                ),
                                            }[theme]
                                        }
                                        <span>
                                            {
                                                {
                                                    light: "Claro",
                                                    dark: "Oscuro",
                                                    system: "Sistema",
                                                }[theme]
                                            }
                                        </span>
                                    </div>
                                </DropdownMenuSubTrigger>
                                <DropdownMenuPortal>
                                    <DropdownMenuSubContent>
                                        <DropdownMenuItem
                                            onClick={(event) =>
                                                onThemeChange(event, "light")
                                            }
                                        >
                                            Claro
                                        </DropdownMenuItem>
                                        <DropdownMenuItem
                                            onClick={(event) =>
                                                onThemeChange(event, "dark")
                                            }
                                        >
                                            Oscuro
                                        </DropdownMenuItem>
                                        <DropdownMenuItem
                                            onClick={(event) =>
                                                onThemeChange(event, "system")
                                            }
                                        >
                                            Sistema
                                        </DropdownMenuItem>
                                    </DropdownMenuSubContent>
                                </DropdownMenuPortal>
                            </DropdownMenuSub>
                        </DropdownMenuGroup>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem
                            onClick={() => void auth.signoutRedirect()}
                        >
                            <LogOutIcon />
                            Cerrar sesión
                        </DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            </SidebarMenuItem>
        </SidebarMenu>
    );
}
