import { DataTable } from "@/shared/components/data-table/data-table";
import type { Table } from "@tanstack/react-table";

type UserDataTableProps<TData> = {
    table: Table<TData>;
};

export default function UserDataTable<TData>({
    table,
}: UserDataTableProps<TData>) {
    return <DataTable table={table} />;
}
