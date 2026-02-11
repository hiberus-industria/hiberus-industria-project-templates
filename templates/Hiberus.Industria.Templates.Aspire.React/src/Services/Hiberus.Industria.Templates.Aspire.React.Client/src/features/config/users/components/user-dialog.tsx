import { Button } from "@/shared/components/ui/button";
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/shared/components/ui/dialog";
import { Form } from "@/shared/components/ui/form";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
    FormField,
    FormItem,
    FormLabel,
    FormControl,
    FormMessage,
} from "@/shared/components/ui/form";
import { Input } from "@/shared/components/ui/input";
import { useState } from "react";
import { PlusIcon } from "lucide-react";
import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "sonner";
import {
    getUsersByIdOptions,
    postUsersMutation,
    putUsersByIdMutation,
} from "@/client/@tanstack/react-query.gen";
import { useUsers } from "@/features/config/users/hooks/use-users";
import { DropdownMenuItem } from "@/shared/components/ui/dropdown-menu";
import type { UserDto } from "@/client";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/shared/components/ui/select";
import { userGroups } from "../helpers/groups";

const formSchema = z.object({
    username: z
        .string()
        .min(1, "El usuario es obligatorio")
        .max(50, "Máximo 50 caracteres"),
    firstName: z
        .string()
        .min(1, "El nombre es obligatorio")
        .max(50, "Máximo 50 caracteres"),
    lastName: z
        .string()
        .min(1, "Los apellidos son obligatorios")
        .max(50, "Máximo 50 caracteres"),
    group: z.string().min(1, "El grupo es obligatorio"),
    email: z
        .string()
        .email("Email no válido")
        .max(100, "Máximo 100 caracteres")
        .optional(),
});

type UserDialogProps = {
    userId?: UserDto["id"];
};

export default function UserDialog({ userId }: UserDialogProps) {
    const [open, setOpen] = useState(false);

    const { invalidateQueries } = useUsers();

    const { data } = useQuery({
        ...getUsersByIdOptions({ path: { id: userId || 0 } }),
        enabled: !!userId && open,
    });

    const { isPending: isCreateUserPending, mutate: mutateCreateUser } =
        useMutation({
            ...postUsersMutation(),
            onSuccess: () => {
                invalidateQueries();
                toast.success("Usuario creado correctamente");
            },
            onSettled: () => {
                form.reset();
                setOpen(false);
            },
            onError: (error) => {
                toast.error(
                    `Error al crear el usuario: ${
                        error.detail?.toLocaleLowerCase() ?? "error desconocido"
                    }`,
                );
            },
        });

    const { isPending: isUpdateUserPending, mutate: mutateUpdateUser } =
        useMutation({
            ...putUsersByIdMutation(),
            onSuccess: () => {
                invalidateQueries();
                toast.success("Usuario actualizado correctamente");
            },
            onSettled: () => {
                form.reset();
                setOpen(false);
            },
            onError: (error) => {
                toast.error(
                    `Error al actualizar el usuario: ${
                        error.detail?.toLocaleLowerCase() ?? "error desconocido"
                    }`,
                );
            },
        });

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        values: {
            username: data?.username || "",
            firstName: data?.firstName || "",
            lastName: data?.lastName || "",
            group: data?.group || "operators",
            email: data?.email || "",
        },
    });

    const isBusy =
        isCreateUserPending ||
        isUpdateUserPending ||
        form.formState.isSubmitting;

    function onOpenChange() {
        form.reset();
        setOpen((prev) => !prev);
    }

    function onSubmit(values: z.infer<typeof formSchema>) {
        if (userId) {
            mutateUpdateUser({
                path: { id: userId },
                body: values,
            });
            return;
        }

        mutateCreateUser({
            body: values,
        });
    }

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <Form {...form}>
                <DialogTrigger asChild>
                    {userId ? (
                        <DropdownMenuItem
                            onSelect={(event) => event.preventDefault()}
                        >
                            Editar
                        </DropdownMenuItem>
                    ) : (
                        <Button
                            variant="outline"
                            className="min-w-[100px]"
                            size="sm"
                        >
                            <PlusIcon />
                            Crear
                        </Button>
                    )}
                </DialogTrigger>
                <DialogContent className="sm:max-w-lg max-h-[85dvh] overflow-y-auto">
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        aria-busy={isBusy}
                    >
                        <fieldset disabled={isBusy} className="space-y-4">
                            <DialogHeader>
                                <DialogTitle>
                                    {userId
                                        ? "Editar usuario"
                                        : "Nuevo usuario"}
                                </DialogTitle>
                                <DialogDescription>
                                    {userId
                                        ? "Edite los datos del usuario."
                                        : "Introduzca los datos del nuevo usuario."}
                                </DialogDescription>
                            </DialogHeader>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                <FormField
                                    control={form.control}
                                    name="username"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Usuario</FormLabel>
                                            <FormControl>
                                                <Input
                                                    placeholder="Usuario"
                                                    {...field}
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="firstName"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Nombre</FormLabel>
                                            <FormControl>
                                                <Input
                                                    placeholder="Nombre"
                                                    {...field}
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="lastName"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Apellidos</FormLabel>
                                            <FormControl>
                                                <Input
                                                    placeholder="Apellidos"
                                                    {...field}
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="email"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Email</FormLabel>
                                            <FormControl>
                                                <Input
                                                    placeholder="Email"
                                                    {...field}
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="group"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Grupo</FormLabel>
                                            <FormControl>
                                                <Select
                                                    value={field.value}
                                                    onValueChange={
                                                        field.onChange
                                                    }
                                                >
                                                    <SelectTrigger className="w-full">
                                                        <SelectValue placeholder="Seleccione un grupo" />
                                                    </SelectTrigger>

                                                    <SelectContent>
                                                        {Object.entries(
                                                            userGroups,
                                                        ).map(
                                                            ([
                                                                value,
                                                                label,
                                                            ]) => (
                                                                <SelectItem
                                                                    key={value}
                                                                    value={
                                                                        value
                                                                    }
                                                                >
                                                                    {label}
                                                                </SelectItem>
                                                            ),
                                                        )}
                                                    </SelectContent>
                                                </Select>
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                            </div>
                            <DialogFooter>
                                <DialogClose asChild>
                                    <Button variant="outline">Cerrar</Button>
                                </DialogClose>

                                <Button type="submit">
                                    {isBusy ? "Guardando..." : "Guardar"}
                                </Button>
                            </DialogFooter>
                        </fieldset>
                    </form>
                </DialogContent>
            </Form>
        </Dialog>
    );
}
