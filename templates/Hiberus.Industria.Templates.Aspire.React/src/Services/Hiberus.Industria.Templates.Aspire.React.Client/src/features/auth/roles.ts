export type Role = "administrator";

export const hasAll = (roles: string[], required: Role[]) =>
    required.every((r) => roles.includes(r));

export const hasAny = (roles: string[], required: Role[]) =>
    required.some((r) => roles.includes(r));

export const hasOne = (roles: string[], role: Role) => roles.includes(role);
