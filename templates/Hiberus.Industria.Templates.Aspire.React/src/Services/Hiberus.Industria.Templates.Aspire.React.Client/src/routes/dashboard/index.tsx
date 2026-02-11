import KpiCardList from "@/features/dashboard/components/kpi-card-list";
import { createFileRoute } from "@tanstack/react-router";

import ReceptionTable from "@/features/dashboard/components/reception-table";
import useLatestReceptions from "@/features/dashboard/hooks/use-latest-receptions";
import useKpis from "@/features/dashboard/hooks/use-kpis";
import KpiCardSkeletonList from "@/features/dashboard/components/kpi-card-skeleton-list";

export const Route = createFileRoute("/(dashboard)/dashboard/")({
    component: RouteComponent,
});

function RouteComponent() {
    const receptions = useLatestReceptions();
    const { isLoading: isKpisLoading, data: kpis } = useKpis();

    return (
        <div className="flex flex-1 flex-col px-4 pb-4 space-y-6">
            {isKpisLoading ? (
                <KpiCardSkeletonList />
            ) : (
                <KpiCardList kpis={kpis} />
            )}

            <div className="flex flex-col gap-2">
                <h2 className="text-xl font-bold tracking-tight">
                    Últimas recepciones
                </h2>
                <ReceptionTable receptions={receptions} />
            </div>
        </div>
    );
}
