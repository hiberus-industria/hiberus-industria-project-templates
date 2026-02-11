import { formatElapsedTime } from "@/shared/lib/format";
import { useState, useEffect } from "react";

export const useElapsedTime = (date?: string | null): string => {
    const [elapsedTime, setElapsedTime] = useState(
        formatElapsedTime(date, new Date().toString()),
    );

    useEffect(() => {
        const interval = setInterval(() => {
            setElapsedTime(formatElapsedTime(date, new Date().toString()));
        }, 1000);

        return () => clearInterval(interval);
    }, [date]);

    return elapsedTime;
};
