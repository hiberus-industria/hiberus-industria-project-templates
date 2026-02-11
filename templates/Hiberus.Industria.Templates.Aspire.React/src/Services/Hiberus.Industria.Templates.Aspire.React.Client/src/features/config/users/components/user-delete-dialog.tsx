import { deleteUsersByIdMutation } from "@/client/@tanstack/react-query.gen";
import {
    AlertDialog,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/shared/components/ui/alert-dialog";
import { DropdownMenuItem } from "@/shared/components/ui/dropdown-menu";
import { useMutation } from "@tanstack/react-query";
import { useUsers } from "@/features/config/users/hooks/use-users";
import { toast } from "sonner";
import { useState } from "react";
import type { UserDto } from "@/client";
import { Button } from "@/shared/components/ui/button";

type UserDeleteDialogProps = {
    userId?: UserDto["id"];
};

export function UserDeleteDialog({ userId }: UserDeleteDialogProps) {
    const [open, setOpen] = useState(false);

    const { invalidateQueries } = useUsers();

    const { isPending, mutate } = useMutation({
        ...deleteUsersByIdMutation(),
        onSuccess: () => {
            invalidateQueries();
            toast.success("Usuario eliminado correctamente");
        },
        onSettled: () => {
            setOpen(false);
        },
        onError: (error) => {
            toast.error(
                `Error al eliminar el usuario: ${
                    error.detail?.toLocaleLowerCase() ?? "error desconocido"
                }`,
            );
        },
    });

    const handleDelete = () => {
        if (!userId) {
            return;
        }

        mutate({ path: { id: userId } });
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <DropdownMenuItem onSelect={(event) => event.preventDefault()}>
                    Eliminar
                </DropdownMenuItem>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>
                        ¿Está seguro de que desea eliminar este usuario?
                    </AlertDialogTitle>
                    <AlertDialogDescription>
                        Esta acción no se puede deshacer. Eliminará
                        permanentemente al usuario y sus datos de nuestros
                        servidores.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancelar</AlertDialogCancel>
                    <Button
                        variant="default"
                        disabled={isPending}
                        onClick={() => handleDelete()}
                    >
                        {isPending ? "Eliminando..." : "Eliminar"}
                    </Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
