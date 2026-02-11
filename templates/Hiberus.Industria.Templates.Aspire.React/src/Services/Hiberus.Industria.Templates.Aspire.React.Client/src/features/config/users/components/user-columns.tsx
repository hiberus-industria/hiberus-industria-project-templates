import type { UserDto } from "@/client";
import { Button } from "@/shared/components/ui/button";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuGroup,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/shared/components/ui/dropdown-menu";
import type { ColumnDef } from "@tanstack/react-table";
import { EllipsisIcon } from "lucide-react";
import UserDialog from "@/features/config/users/components/user-dialog";
import { UserDeleteDialog } from "@/features/config/users/components/user-delete-dialog";
import { UserResetPasswordDialog } from "@/features/config/users/components/user-reset-password-dialog";
import { userGroups } from "../helpers/groups";

export function getUserColumns(): ColumnDef<UserDto>[] {
    return [
        {
            id: "username",
            accessorKey: "username",
            header: "Usuario",
            meta: {
                label: "Usuario",
                placeholder: "Ingrese un usuario...",
                variant: "text",
            },
            enableColumnFilter: true,
        },
        {
            id: "email",
            accessorKey: "email",
            header: "Email",
            meta: {
                label: "Email",
                placeholder: "Ingrese un correo...",
                variant: "text",
            },
            enableColumnFilter: false,
        },
        {
            id: "firstName",
            accessorKey: "firstName",
            header: "Nombre",
            meta: {
                label: "Nombre",
                placeholder: "Ingrese un nombre...",
                variant: "text",
            },
            enableColumnFilter: false,
        },
        {
            id: "lastName",
            accessorKey: "lastName",
            header: "Apellidos",
            meta: {
                label: "Apellidos",
                placeholder: "Ingrese un apellido...",
                variant: "text",
            },
            enableColumnFilter: false,
        },
        {
            id: "group",
            accessorFn: (row) => (row.group ? userGroups[row.group] : "N/A"),
            header: "Grupo",
            meta: {
                label: "Grupo",
                placeholder: "Seleccione un grupo",
                variant: "multiSelect",
                options: Object.entries(userGroups).map(([value, label]) => ({
                    value,
                    label,
                })),
            },
            enableColumnFilter: true,
        },
        {
            id: "actions",
            cell: function Cell({ row }) {
                const user = row.original;
                const userId = user.id;

                return (
                    <DropdownMenu modal={false}>
                        <DropdownMenuTrigger asChild>
                            <Button
                                aria-label="Abrir menú de acciones"
                                variant="ghost"
                                className="flex size-8 p-0 data-[state=open]:bg-muted"
                            >
                                <EllipsisIcon
                                    className="size-4"
                                    aria-hidden="true"
                                />
                            </Button>
                        </DropdownMenuTrigger>

                        <DropdownMenuContent align="end" className="w-40">
                            <DropdownMenuGroup>
                                <UserDialog userId={userId} />
                                <UserResetPasswordDialog userId={userId} />
                            </DropdownMenuGroup>

                            <DropdownMenuSeparator />

                            <DropdownMenuGroup>
                                <UserDeleteDialog userId={userId} />
                            </DropdownMenuGroup>
                        </DropdownMenuContent>
                    </DropdownMenu>
                );
            },
        },
    ];
}
