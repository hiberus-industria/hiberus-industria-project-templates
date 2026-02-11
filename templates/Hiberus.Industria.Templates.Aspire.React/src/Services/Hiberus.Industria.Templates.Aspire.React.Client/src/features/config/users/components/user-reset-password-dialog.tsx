import { postUsersByIdResetPasswordMutation } from "@/client/@tanstack/react-query.gen";
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
import {
    Alert,
    AlertDescription,
    AlertTitle,
} from "@/shared/components/ui/alert";
import { InfoIcon } from "lucide-react";
import type { UserDto } from "@/client";
import { Button } from "@/shared/components/ui/button";

type UserResetPasswordDialogProps = {
    userId?: UserDto["id"];
};

export function UserResetPasswordDialog({
    userId,
}: UserResetPasswordDialogProps) {
    const [open, setOpen] = useState(false);

    const { invalidateQueries } = useUsers();

    const { isPending, mutate } = useMutation({
        ...postUsersByIdResetPasswordMutation(),
        onSuccess: () => {
            invalidateQueries();
            toast.success("Contraseña reestablecida correctamente");
        },
        onSettled: () => {
            setOpen(false);
        },
        onError: (error) => {
            toast.error(
                `Error al reestablecer la contraseña: ${
                    error.detail?.toLocaleLowerCase() ?? "error desconocido"
                }`,
            );
        },
    });

    const handleResetPassword = () => {
        if (!userId) {
            return;
        }

        mutate({ path: { id: userId } });
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <DropdownMenuItem onSelect={(event) => event.preventDefault()}>
                    Restablecer contraseña
                </DropdownMenuItem>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>
                        ¿Está seguro de que desea restablecer la contraseña?
                    </AlertDialogTitle>
                    <AlertDialogDescription>
                        <div className="flex flex-col gap-2">
                            Esta acción no se puede deshacer.
                            <Alert variant="default">
                                <InfoIcon />
                                <AlertTitle>Atención</AlertTitle>
                                <AlertDescription>
                                    La contraseña se restablecerá a un valor por
                                    defecto y el usuario deberá cambiarla en su
                                    próximo inicio de sesión. Como
                                    administrador, transmita esta información
                                    por el canal adecuado.
                                </AlertDescription>
                            </Alert>
                        </div>
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancelar</AlertDialogCancel>
                    <Button
                        variant="default"
                        disabled={isPending}
                        onClick={() => handleResetPassword()}
                    >
                        {isPending ? "Restableciendo..." : "Restablecer"}
                    </Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
