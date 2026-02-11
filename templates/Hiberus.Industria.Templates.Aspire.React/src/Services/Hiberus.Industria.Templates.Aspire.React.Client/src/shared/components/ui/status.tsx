import type { ComponentProps, HTMLAttributes } from "react";
import { Badge } from "@/shared/components/ui/badge";
import { cn } from "@/shared/lib/utils";

export type StatusProps = ComponentProps<typeof Badge> & {
    status: "online" | "offline" | "maintenance" | "degraded";
};

export const Status = ({ className, status, ...props }: StatusProps) => (
    <Badge
        className={cn("flex items-center gap-2", "group", status, className)}
        variant="secondary"
        {...props}
    />
);

export type StatusIndicatorProps = HTMLAttributes<HTMLSpanElement>;

export const StatusIndicator = ({
    className,
    ...props
}: StatusIndicatorProps) => (
    <span className="relative flex h-2 w-2" {...props}>
        <span
            className={cn(
                "absolute inline-flex h-full w-full animate-ping rounded-full opacity-75",
                "group-[.online]:bg-emerald-600 dark:group-[.online]:bg-emerald-400",
                "group-[.offline]:bg-red-600 dark:group-[.offline]:bg-red-400",
                "group-[.maintenance]:bg-blue-600 dark:group-[.maintenance]:bg-blue-400",
                "group-[.degraded]:bg-amber-600 dark:group-[.degraded]:bg-amber-400",
            )}
        />
        <span
            className={cn(
                "relative inline-flex h-2 w-2 rounded-full",
                "group-[.online]:bg-emerald-600 dark:group-[.online]:bg-emerald-400",
                "group-[.offline]:bg-red-600 dark:group-[.offline]:bg-red-400",
                "group-[.maintenance]:bg-blue-600 dark:group-[.maintenance]:bg-blue-400",
                "group-[.degraded]:bg-amber-600 dark:group-[.degraded]:bg-amber-400",
            )}
        />
    </span>
);

export type StatusLabelProps = HTMLAttributes<HTMLSpanElement>;

export const StatusLabel = ({
    className,
    children,
    ...props
}: StatusLabelProps) => (
    <span className={cn("text-muted-foreground", className)} {...props}>
        {children ?? (
            <>
                <span className="hidden group-[.online]:block">Conectado</span>
                <span className="hidden group-[.offline]:block">
                    Desconectado
                </span>
                <span className="hidden group-[.maintenance]:block">
                    Mantenimiento
                </span>
                <span className="hidden group-[.degraded]:block">
                    Degradado
                </span>
            </>
        )}
    </span>
);
