import type { ProblemDetails, UserDto } from "@/client";
import { getUsersOptions } from "@/client/@tanstack/react-query.gen";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {
    parseAsArrayOf,
    parseAsInteger,
    parseAsString,
    useQueryStates,
} from "nuqs";
import { useMemo } from "react";
import { z } from "zod";

const DEFAULT_PAGE_SIZE = 20;

// Define the zod schema for filters
const filtersSchema = z
    .object({
        page: z.number().min(1).default(1),
        pageSize: z.number().min(1).default(DEFAULT_PAGE_SIZE),
        group: z.array(z.string()).optional(),
        username: z.string().optional(),
    })
    .strict();

// Define parsers for useQueryStates
const filtersParsers = {
    page: parseAsInteger.withDefault(1),
    pageSize: parseAsInteger.withDefault(DEFAULT_PAGE_SIZE),
    group: parseAsArrayOf(parseAsString).withDefault([]),
    username: parseAsString.withDefault(""),
};

type Filters = z.infer<typeof filtersSchema>;

// Interface for the hook's return type
interface UsersResult {
    data: UserDto[];
    totalCount?: number;
    totalPages?: number;
    isLoading: boolean;
    isFetching: boolean;
    error: Error | ProblemDetails | null;
    invalidateQueries: () => void;
}

/**
 * Custom hook to fetch users data.
 * @returns UsersResult containing data, pagination info, and query controls.
 */
export function useUsers(): UsersResult {
    const queryClient = useQueryClient();
    const [filters] = useQueryStates(filtersParsers);

    // Validate filters with defaults
    const validatedFilters: Filters = useMemo(
        () =>
            filtersSchema.parse(
                filtersSchema.safeParse(filters).success ? filters : {},
            ),
        [filters],
    );

    const queryOptions = getUsersOptions({ query: validatedFilters });
    const queryKey = queryOptions.queryKey;

    const queryResult = useQuery({
        ...queryOptions,
        placeholderData: (previous) => previous,
        staleTime: 1000 * 60 * 5, // 5 minutes
    });

    const invalidateQueries = () => {
        queryClient.invalidateQueries({ queryKey });
    };

    return {
        data: queryResult.data?.items ?? [],
        totalCount: queryResult.data?.totalCount,
        totalPages: queryResult.data?.totalPages,
        isLoading: queryResult.isLoading,
        isFetching: queryResult.isFetching,
        error: queryResult.error,
        invalidateQueries,
    };
}
