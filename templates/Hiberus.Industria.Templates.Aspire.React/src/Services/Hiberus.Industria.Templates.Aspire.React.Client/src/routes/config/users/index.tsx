import { getUserColumns } from "@/features/config/users/components/user-columns";
import UserDataTable from "@/features/config/users/components/user-data-table";
import UserToolbar from "@/features/config/users/components/user-toolbar";
import { useUsers } from "@/features/config/users/hooks/use-users";
import { useDataTable } from "@/shared/hooks/use-data-table";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/(dashboard)/config/users/")({
    component: RouteComponent,
});

function RouteComponent() {
    const { data: users, totalPages } = useUsers();

    const { table } = useDataTable({
        data: users,
        columns: getUserColumns(),
        pageCount: totalPages ?? 0,
        getRowId: (row) => row?.id?.toString() ?? "",
        shallow: false,
        clearOnDefault: true,
        initialState: {
            columnPinning: {
                right: ["actions"],
            },
        },
    });

    return (
        <div className="flex flex-1 flex-col space-y-4">
            <UserToolbar table={table} />
            <UserDataTable table={table} />
        </div>
    );
}
