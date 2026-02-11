import { DataTableToolbar } from "@/shared/components/data-table/data-table-toolbar";
import type { Table } from "@tanstack/react-table";
import UserDialog from "@/features/config/users/components/user-dialog";

type UserToolbarProps<TData> = {
    table: Table<TData>;
};

export default function UserToolbar<TData>({ table }: UserToolbarProps<TData>) {
    return (
        <DataTableToolbar table={table}>
            <UserDialog />
        </DataTableToolbar>
    );
}
