import { format, isValid, parseISO } from "date-fns";

export function formatDate(
    date: Date | string | number | null | undefined,
    opts: Intl.DateTimeFormatOptions = {},
) {
    if (!date) return null;

    try {
        // Verify if only time elements are requested.
        const isTimeOnly =
            (opts.hour !== undefined ||
                opts.minute !== undefined ||
                opts.second !== undefined) &&
            opts.month === undefined &&
            opts.day === undefined &&
            opts.year === undefined;

        return new Intl.DateTimeFormat("es-ES", {
            // If it is only time, do not include default date values.
            month: isTimeOnly ? undefined : (opts.month ?? "long"),
            day: isTimeOnly ? undefined : (opts.day ?? "numeric"),
            year: isTimeOnly ? undefined : (opts.year ?? "numeric"),
            ...opts,
        }).format(new Date(date));
    } catch (_err) {
        return null;
    }
}

export const formatElapsedTime = (
    startDate?: string | null,
    endDate?: string | null,
): string => {
    if (!startDate || !endDate) return "--:--";

    const startMs = new Date(startDate).getTime();
    const endMs = new Date(endDate).getTime();
    const diffSeconds = Math.floor((endMs - startMs) / 1000);
    const minutes = Math.floor(diffSeconds / 60)
        .toString()
        .padStart(2, "0");
    const seconds = (diffSeconds % 60).toString().padStart(2, "0");
    return `${minutes}:${seconds}`;
};

// Converts a date for the form (string | null | undefined to Date | undefined) or for the API (Date | undefined to string | null)
export function handleDate(value: string | null | undefined): Date | undefined;
export function handleDate(value: Date | undefined): string | null;
export function handleDate(
    value: string | null | undefined | Date,
): Date | string | undefined | null {
    if (typeof value === "string") {
        // For the form: converts ISO string to Date or undefined if invalid
        if (!value) return undefined;
        const parsed = parseISO(value);
        return isValid(parsed) ? parsed : undefined;
    }
    if (value === null) {
        // For the form: handles null as undefined
        return undefined;
    }
    // For the API: converts Date to string yyyy-MM-dd or null if no value
    return value && isValid(value) ? format(value, "yyyy-MM-dd") : null;
}

// Converts seconds to a formatted string HH:mm
export function formatSecondsToHoursAndMinutes(seconds: number): string {
    const baseDate = new Date(0); // Epoch
    const time = new Date(baseDate.getTime() + seconds * 1000);
    return format(time, "HH:mm");
}

export function formatRelativeDate(dateString: string): string {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return "Ahora mismo";
    if (diffMins < 60) return `Hace ${diffMins}m`;
    if (diffHours < 24) return `Hace ${diffHours}h`;
    if (diffDays < 7) return `Hace ${diffDays}d`;

    return date.toLocaleDateString("es-ES", {
        month: "short",
        day: "numeric",
        year: date.getFullYear() !== now.getFullYear() ? "numeric" : undefined,
        hour: "2-digit",
        minute: "2-digit",
    });
}
