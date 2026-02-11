import { useEffect, useState } from "react";

export function useCurrentTime() {
    const [now, setNow] = useState(new Date());

    useEffect(() => {
        // Solo actualiza al cambiar el minuto
        const interval = setInterval(() => {
            const newNow = new Date();
            if (newNow.getMinutes() !== now.getMinutes()) {
                setNow(newNow);
            }
        }, 1000);
        return () => clearInterval(interval);
    }, [now]);

    return now;
}
