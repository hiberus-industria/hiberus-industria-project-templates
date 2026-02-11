import type {
    ProblemDetails,
    ReadingFrameExternalStatusType,
    ReadingFrameStatusType,
    ReadingFrameWithDetailsDto,
} from "@/client";
import { getReadingFramesOptions } from "@/client/@tanstack/react-query.gen";
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
        warehouseId: z.array(z.coerce.number()).optional(),
        dockId: z.array(z.coerce.number()).optional(),
        code: z.string().optional(),
        status: z.array(z.custom<ReadingFrameStatusType>()).optional(),
        externalStatus: z
            .array(z.custom<ReadingFrameExternalStatusType>())
            .optional(),
    })
    .strict();

// Define parsers for useQueryStates
const filtersParsers = {
    page: parseAsInteger.withDefault(1),
    pageSize: parseAsInteger.withDefault(DEFAULT_PAGE_SIZE),
    warehouseId: parseAsArrayOf(parseAsInteger).withDefault([]),
    dockId: parseAsArrayOf(parseAsInteger).withDefault([]),
    code: parseAsString.withDefault(""),
    status: parseAsArrayOf(parseAsString).withDefault([]),
    externalStatus: parseAsArrayOf(parseAsString).withDefault([]),
};

type Filters = z.infer<typeof filtersSchema>;

// Interface for the hook's return type
interface ReadingFramesResult {
    data: ReadingFrameWithDetailsDto[];
    totalCount?: number;
    totalPages?: number;
    isLoading: boolean;
    isFetching: boolean;
    error: Error | ProblemDetails | null;
    invalidateQueries: () => void;
}

/**
 * Custom hook to fetch reading frames data.
 * @returns ReadingFramesResult containing data, pagination info, and query controls.
 */
export function useReadingFrames(): ReadingFramesResult {
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
    const queryOptions = getReadingFramesOptions({ query: validatedFilters });
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
