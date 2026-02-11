import { ReceptionStatusType } from "@/client";
import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs));
}

export function getIfReceptionReadOnly(status?: ReceptionStatusType): boolean {
    return [2, 3].includes(status ?? -1);
}

export function getReceptionStatusLabel(status: number): string {
    const labels: Record<number, string> = {
        0: "Pendiente",
        1: "En curso",
        2: "Completado",
        3: "Cancelado",
    };
    return labels[status] ?? "Desconocido";
}

export function getReadingFrameStatusLabel(status: number): string {
    const labels: Record<number, string> = {
        0: "Disponible",
        1: "Activo",
        2: "Mantenimiento",
    };
    return labels[status] ?? "Desconocido";
}
export function getReadingFrameTypeLabel(status: number): string {
    const labels: Record<number, string> = {
        0: "Manual",
        1: "Automatico",
    };
    return labels[status] ?? "Desconocido";
}

export function getReadingFrameExternalStatusLabel(status: number): string {
    const labels: Record<number, string> = {
        0: "Inactivo",
        1: "Activo",
        2: "Error integración",
    };
    return labels[status] ?? "Desconocido";
}

// Gets the initials (firstName and lastName)
export function getInitialsFromFullName(fullName?: string): string {
    if (!fullName) return "";
    const names = fullName.split(" ");
    const initials = names.map((name) => name.charAt(0)).join("");
    return initials;
}
export function getRequirementLabel(requirement: number): string {
    const labels: Record<number, string> = {
        0: "Optional",
        1: "Mandatory",
    };
    return labels[requirement] ?? "Unknown";
}
